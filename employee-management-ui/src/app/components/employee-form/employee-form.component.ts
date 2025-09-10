import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EmployeePersonal, EmployeeProfessional, EmployeeUpsertRequest } from '../../models/employee.model';
import { EmployeeService } from '../../services/employee.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';

interface DialogData {
  mode: 'add' | 'edit';
  employee?: EmployeePersonal;
}

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatProgressSpinnerModule
  ]
})
export class EmployeeFormComponent implements OnInit {
  employeeForm!: FormGroup;
  personalForm!: FormGroup;
  professionalForm!: FormGroup;
  isEditMode = false;
  isLoading = false;
  selectedImage: File | null = null;
  imagePreview: string | null = null;
  selectedDocument: File | null = null;
  documentPreview: string | null = null;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private dialogRef: MatDialogRef<EmployeeFormComponent>,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.isEditMode = data.mode === 'edit';
    this.initializeForms();
  }

  ngOnInit(): void {
    if (this.isEditMode && this.data.employee) {
      this.loadEmployeeData();
    }
  }

  private initializeForms(): void {
    this.personalForm = this.fb.group({
      employeeId: [0],
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      gender: ['M', Validators.required],
      dob: ['', Validators.required],
      age: [null],
      address: ['', Validators.required],
      contactNo: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]+$/)]],
      email: ['', [Validators.required, Validators.email]],
      profileImageUrl: ['']
    });

    this.professionalForm = this.fb.group({
      employeeId: [0],
      designation: ['', Validators.required],
      department: ['', Validators.required],
      qualification: [''],
      experience: [null, [Validators.min(0)]],
      skill: ['']
    });

    this.employeeForm = this.fb.group({
      personal: this.personalForm,
      professional: this.professionalForm
    });

    // Auto-calculate age when DOB changes
    this.personalForm.get('dob')?.valueChanges.subscribe(dob => {
      if (dob) {
        const age = this.calculateAge(new Date(dob));
        this.personalForm.patchValue({ age });
      }
    });
  }

  private loadEmployeeData(): void {
    if (this.data.employee) {
      const employee = this.data.employee;
      this.personalForm.patchValue({
        employeeId: employee.employeeId,
        fullName: employee.fullName,
        gender: employee.gender,
        dob: employee.dob,
        age: employee.age,
        address: employee.address,
        contactNo: employee.contactNo,
        email: employee.email,
        profileImageUrl: employee.profileImageUrl
      });

      this.professionalForm.patchValue({
        employeeId: employee.employeeId
      });

      if (employee.profileImageUrl) {
        this.imagePreview = employee.profileImageUrl;
      }
    }
  }

  private calculateAge(birthDate: Date): number {
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    
    return age;
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedImage = file;
      
      // Create preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onDocumentSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedDocument = file;
      this.documentPreview = file.name;
    }
  }

  removeImage(): void {
    this.selectedImage = null;
    this.imagePreview = null;
    this.personalForm.patchValue({ profileImageUrl: '' });
  }

  removeDocument(): void {
    this.selectedDocument = null;
    this.documentPreview = null;
  }

  onSubmit(): void {
    if (this.employeeForm.valid) {
      this.isLoading = true;
      const formData = this.employeeForm.value;
      
      const employeeRequest: EmployeeUpsertRequest = {
        personal: formData.personal,
        professional: formData.professional
      };

      const operation = this.isEditMode 
        ? this.employeeService.updateEmployee(formData.personal.employeeId, employeeRequest)
        : this.employeeService.createEmployee(employeeRequest);

      operation.subscribe({
        next: (response) => {
          const employeeId = this.isEditMode ? formData.personal.employeeId : response.employeeId;
          
          // Upload image if selected
          if (this.selectedImage && employeeId) {
            this.employeeService.uploadImage(employeeId, this.selectedImage).subscribe({
              next: () => {
                this.isLoading = false;
                this.snackBar.open(
                  `Employee ${this.isEditMode ? 'updated' : 'created'} successfully!`, 
                  'Close', 
                  { duration: 3000 }
                );
                this.dialogRef.close(true);
              },
              error: (error) => {
                this.isLoading = false;
                this.snackBar.open('Error uploading image', 'Close', { duration: 3000 });
                console.error('Error uploading image:', error);
              }
            });
          } else {
            this.isLoading = false;
            this.snackBar.open(
              `Employee ${this.isEditMode ? 'updated' : 'created'} successfully!`, 
              'Close', 
              { duration: 3000 }
            );
            this.dialogRef.close(true);
          }

          // Upload document if selected
          if (this.selectedDocument && employeeId) {
            this.employeeService.uploadDocument(employeeId, this.selectedDocument).subscribe({
              error: (error) => {
                console.error('Error uploading document:', error);
              }
            });
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.snackBar.open(
            `Error ${this.isEditMode ? 'updating' : 'creating'} employee`, 
            'Close', 
            { duration: 3000 }
          );
          console.error('Error saving employee:', error);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.personalForm.controls).forEach(key => {
      this.personalForm.get(key)?.markAsTouched();
    });
    Object.keys(this.professionalForm.controls).forEach(key => {
      this.professionalForm.get(key)?.markAsTouched();
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  getFieldError(form: FormGroup, fieldName: string): string {
    const field = form.get(fieldName);
    if (field?.hasError('required')) {
      return `${fieldName} is required`;
    }
    if (field?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    if (field?.hasError('pattern')) {
      return 'Please enter a valid phone number';
    }
    if (field?.hasError('minlength')) {
      return `${fieldName} must be at least ${field.errors?.['minlength'].requiredLength} characters`;
    }
    if (field?.hasError('min')) {
      return `${fieldName} must be greater than or equal to ${field.errors?.['min'].min}`;
    }
    return '';
  }
}