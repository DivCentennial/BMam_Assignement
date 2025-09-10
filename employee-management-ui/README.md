# Employee Management System - Angular Frontend

A modern Angular 17 application for managing employee data with a beautiful Material Design UI.

## Features

### 🔐 Authentication
- **Login Screen**: Clean, modern login interface with validation
- **Role-based Access**: Support for Admin and User roles
- **Session Management**: Persistent login with localStorage
- **Logout Functionality**: Secure logout with redirect to login

### 👥 Employee Management
- **Employee Listing**: Sortable and filterable data table
- **Add Employee**: Modal form with personal and professional information
- **Edit Employee**: Update existing employee records
- **Delete Employee**: Remove employees (Admin only)
- **Profile Images**: Upload and preview employee photos
- **Document Management**: Upload, download, and delete profile documents

### 📊 Data Management
- **CSV Export**: Export all employee data to CSV format
- **CSV Import**: Import employee data from CSV files
- **Real-time Validation**: Form validation with error messages
- **Auto-calculated Age**: Age automatically calculated from date of birth

### 🎨 UI/UX Features
- **Material Design**: Modern, responsive Material Design components
- **Responsive Layout**: Works on desktop, tablet, and mobile devices
- **Loading States**: Spinners and loading indicators
- **Success/Error Messages**: Toast notifications for user feedback
- **Image Preview**: Real-time image preview during upload
- **Form Validation**: Comprehensive form validation with error messages

## Technology Stack

- **Frontend**: Angular 17 with standalone components
- **UI Framework**: Angular Material Design
- **Styling**: SCSS with custom themes
- **Forms**: Reactive Forms with validation
- **HTTP Client**: Angular HttpClient for API communication
- **Routing**: Angular Router with guards
- **State Management**: Services with BehaviorSubject

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── login/                 # Login component
│   │   ├── employee-list/         # Employee listing with table
│   │   ├── employee-form/         # Add/Edit employee modal
│   │   └── navbar/                # Navigation bar
│   ├── services/
│   │   ├── auth.service.ts        # Authentication service
│   │   └── employee.service.ts    # Employee CRUD operations
│   ├── models/
│   │   ├── user-auth.model.ts     # User authentication models
│   │   └── employee.model.ts      # Employee data models
│   ├── guards/
│   │   └── auth.guard.ts          # Route protection
│   ├── app.component.ts           # Main app component
│   ├── app.config.ts              # App configuration
│   └── app.routes.ts              # Routing configuration
├── styles.scss                    # Global styles and Material theme
└── main.ts                        # Application bootstrap
```

## Getting Started

### Prerequisites
- Node.js (v18 or higher)
- Angular CLI (v17 or higher)
- Your C# backend API running

### Installation

1. **Navigate to the project directory:**
   ```bash
   cd employee-management-ui
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Update API URL:**
   Open `src/app/services/auth.service.ts` and `src/app/services/employee.service.ts`
   Update the `apiUrl` to match your backend API URL:
   ```typescript
   private apiUrl = 'https://localhost:7000/api'; // Update this URL
   ```

4. **Start the development server:**
   ```bash
   ng serve
   ```

5. **Open your browser:**
   Navigate to `http://localhost:4200`

## API Integration

The application expects your C# backend to be running with the following endpoints:

### Authentication
- `POST /api/auth/login` - User login

### Employee Management
- `GET /api/employee` - Get all employees
- `GET /api/employee/{id}` - Get employee by ID
- `POST /api/employee` - Create new employee (Admin only)
- `PUT /api/employee/{id}` - Update employee (Admin only)
- `DELETE /api/employee/{id}` - Delete employee (Admin only)

### File Management
- `POST /api/employee/{id}/image` - Upload profile image (Admin only)
- `GET /api/employee/{id}/image` - Download profile image
- `DELETE /api/employee/{id}/image` - Delete profile image (Admin only)
- `POST /api/employee/{id}/document` - Upload profile document (Admin only)
- `GET /api/employee/{id}/document` - Download profile document
- `DELETE /api/employee/{id}/document` - Delete profile document (Admin only)

### Data Export/Import
- `GET /api/employee/export/csv` - Export employees to CSV
- `POST /api/employee/import/csv` - Import employees from CSV

## Features in Detail

### Login Screen
- Username and password validation
- Login button only enabled when both fields are filled
- Error handling for invalid credentials
- Beautiful gradient background with Material Design card

### Employee Listing
- Material Design data table with sorting
- Search/filter functionality
- Profile image thumbnails
- Role-based action buttons (Admin can add/edit/delete)
- Export to CSV functionality
- Import from CSV functionality

### Employee Form (Add/Edit)
- **Personal Information Tab:**
  - Full name, gender, date of birth
  - Auto-calculated age from DOB
  - Email with validation
  - Contact number with pattern validation
  - Address field
  - Profile image upload with preview

- **Professional Information Tab:**
  - Designation and department (required)
  - Qualification and experience
  - Skills field
  - Profile document upload

### Responsive Design
- Mobile-first approach
- Responsive table that adapts to screen size
- Touch-friendly buttons and inputs
- Optimized for tablets and phones

## Customization

### Styling
- Modify `src/styles.scss` for global styles
- Update Material theme colors in the same file
- Component-specific styles in individual `.scss` files

### API Configuration
- Update API URLs in service files
- Modify request headers if needed
- Add additional endpoints as required

## Build for Production

```bash
ng build --configuration production
```

The build artifacts will be stored in the `dist/` directory.

## Development

### Code Structure
- **Standalone Components**: All components are standalone for better tree-shaking
- **Reactive Forms**: Form validation and handling
- **Services**: Centralized business logic and API communication
- **Guards**: Route protection and authentication
- **Models**: TypeScript interfaces for type safety

### Best Practices Implemented
- Lazy loading with standalone components
- Proper error handling and user feedback
- Form validation with meaningful error messages
- Responsive design principles
- Material Design guidelines
- TypeScript strict mode
- Clean code architecture

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure your backend API has CORS configured for the Angular app URL
2. **API Connection**: Verify the API URL in service files matches your backend
3. **Authentication**: Check that the backend returns the expected user object structure
4. **File Uploads**: Ensure backend supports multipart/form-data for file uploads

### Browser Compatibility
- Chrome (recommended)
- Firefox
- Safari
- Edge

## Contributing

1. Follow Angular style guide
2. Use TypeScript strict mode
3. Implement proper error handling
4. Add meaningful comments
5. Test on multiple devices/browsers

## License

This project is part of the Employee Management System training project.