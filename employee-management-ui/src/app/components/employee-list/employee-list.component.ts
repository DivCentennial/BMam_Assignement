import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { EmployeePersonal } from '../../models/employee.model';
import { EmployeeService } from '../../services/employee.service';
import { AuthService } from '../../services/auth.service';
import { EmployeeFormComponent } from '../employee-form/employee-form.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ]
})
export class EmployeeListComponent implements OnInit {
  displayedColumns: string[] = ['employeeId', 'fullName', 'gender', 'dob', 'age', 'email', 'contactNo', 'actions'];
  dataSource = new MatTableDataSource<EmployeePersonal>();
  isLoading = false;
  searchTerm = '';

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getAllEmployees().subscribe({
      next: (employees) => {
        this.dataSource.data = employees;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.snackBar.open('Error loading employees', 'Close', { duration: 3000 });
        console.error('Error loading employees:', error);
      }
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(EmployeeFormComponent, {
      width: '800px',
      data: { mode: 'add' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEmployees();
      }
    });
  }

  openEditDialog(employee: EmployeePersonal): void {
    const dialogRef = this.dialog.open(EmployeeFormComponent, {
      width: '800px',
      data: { mode: 'edit', employee }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEmployees();
      }
    });
  }

  deleteEmployee(employee: EmployeePersonal): void {
    if (confirm(`Are you sure you want to delete ${employee.fullName}?`)) {
      this.employeeService.deleteEmployee(employee.employeeId).subscribe({
        next: () => {
          this.snackBar.open('Employee deleted successfully', 'Close', { duration: 3000 });
          this.loadEmployees();
        },
        error: (error) => {
          this.snackBar.open('Error deleting employee', 'Close', { duration: 3000 });
          console.error('Error deleting employee:', error);
        }
      });
    }
  }

  exportToCsv(): void {
    this.employeeService.exportCsv().subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'employees.csv';
        link.click();
        window.URL.revokeObjectURL(url);
        this.snackBar.open('Export completed successfully', 'Close', { duration: 3000 });
      },
      error: (error) => {
        this.snackBar.open('Error exporting data', 'Close', { duration: 3000 });
        console.error('Error exporting data:', error);
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.employeeService.importCsv(file).subscribe({
        next: (result) => {
          this.snackBar.open(`Successfully imported ${result.imported} employees`, 'Close', { duration: 3000 });
          this.loadEmployees();
        },
        error: (error) => {
          this.snackBar.open('Error importing data', 'Close', { duration: 3000 });
          console.error('Error importing data:', error);
        }
      });
    }
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}