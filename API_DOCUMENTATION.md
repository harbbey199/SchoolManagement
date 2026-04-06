# API Endpoints Documentation

## Base URL
- **Development**: `http://localhost:5000` or `https://localhost:5001`
- **Production**: `https://api.schoolmanagement.com`

## Authentication
All endpoints (except `/auth/register` and `/auth/login`) require JWT token in the Authorization header:

```
Authorization: Bearer {token}
```

## Response Format
All API responses follow this format:

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {}
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "data": null
}
```

### Paginated Response
```json
{
  "success": true,
  "message": "Data retrieved",
  "data": [],
  "total": 100,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

---

## Authentication Endpoints

### Register User
**POST** `/api/auth/register`

Register a new user account.

**Request Body**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "role": 0
}
```

**Role Values**
- 0 = Admin
- 1 = Parent
- 2 = Student
- 3 = Teacher

**Response (201 Created)**
```json
{
  "success": true,
  "message": "Registration successful",
  "data": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Admin",
    "token": ""
  }
}
```

**Error Response (400 Bad Request)**
```json
{
  "success": false,
  "message": "Email already exists",
  "data": null
}
```

### Login
**POST** `/api/auth/login`

Authenticate user and receive JWT token.

**Request Body**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Admin",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

**Error Response (401 Unauthorized)**
```json
{
  "success": false,
  "message": "Invalid credentials",
  "data": null
}
```

---

## Student Endpoints

### Get All Students (Paginated)
**GET** `/api/students?pageNumber=1&pageSize=10`

Retrieve all students with pagination support.

**Query Parameters**
- `pageNumber` (optional): Page number, default = 1
- `pageSize` (optional): Items per page, default = 10, max = 100

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Students retrieved",
  "data": [
    {
      "id": 1,
      "userId": 1,
      "email": "student1@school.com",
      "firstName": "Alice",
      "lastName": "Johnson",
      "rollNumber": "STU001",
      "grade": "10A",
      "section": "A",
      "dateOfBirth": "2008-05-15",
      "address": "123 Main St"
    }
  ],
  "total": 50,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Get Student Details
**GET** `/api/students/{id}`

Retrieve detailed information for a specific student.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": 1,
    "email": "student1@school.com",
    "firstName": "Alice",
    "lastName": "Johnson",
    "rollNumber": "STU001",
    "grade": "10A",
    "section": "A",
    "dateOfBirth": "2008-05-15",
    "address": "123 Main St",
    "parents": [
      {
        "id": 1,
        "firstName": "Robert",
        "lastName": "Johnson",
        "email": "robert@email.com",
        "phoneNumber": "9876543210",
        "relationship": "Father"
      }
    ],
    "attendancePercentage": 92.5,
    "averageGrade": 85.5
  }
}
```

### Get Students by Grade
**GET** `/api/students/grade/{grade}`

Retrieve all students in a specific grade.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "userId": 1,
      "email": "student1@school.com",
      "firstName": "Alice",
      "lastName": "Johnson",
      "rollNumber": "STU001",
      "grade": "10A",
      "section": "A",
      "dateOfBirth": "2008-05-15",
      "address": "123 Main St"
    }
  ]
}
```

### Create Student
**POST** `/api/students`

Create a new student (Admin only).

**Request Body**
```json
{
  "email": "newstudent@school.com",
  "firstName": "Bob",
  "lastName": "Smith",
  "password": "SecurePassword123!",
  "rollNumber": "STU002",
  "grade": "10B",
  "section": "B",
  "dateOfBirth": "2008-06-20",
  "address": "456 Oak Ave"
}
```

**Response (201 Created)**
```json
{
  "success": true,
  "message": "Student created successfully",
  "data": {
    "id": 2,
    "userId": 2,
    "email": "newstudent@school.com",
    "firstName": "Bob",
    "lastName": "Smith",
    "rollNumber": "STU002",
    "grade": "10B",
    "section": "B",
    "dateOfBirth": "2008-06-20",
    "address": "456 Oak Ave"
  }
}
```

### Update Student
**PUT** `/api/students/{id}`

Update student information (Admin only).

**Request Body**
```json
{
  "firstName": "Robert",
  "lastName": "Smith",
  "grade": "11A",
  "section": "A",
  "dateOfBirth": "2008-06-20",
  "address": "789 Pine Rd"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Student updated successfully",
  "data": {
    "id": 2,
    "userId": 2,
    "email": "newstudent@school.com",
    "firstName": "Robert",
    "lastName": "Smith",
    "rollNumber": "STU002",
    "grade": "11A",
    "section": "A",
    "dateOfBirth": "2008-06-20",
    "address": "789 Pine Rd"
  }
}
```

### Delete Student
**DELETE** `/api/students/{id}`

Delete a student (soft delete, Admin only).

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Student deleted successfully",
  "data": null
}
```

### Assign Parent to Student
**POST** `/api/students/{studentId}/parents`

Assign a parent to a student (Admin only).

**Request Body**
```json
{
  "parentId": 1,
  "relationship": "Mother"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Parent assigned successfully",
  "data": null
}
```

---

## Attendance Endpoints

### Mark Attendance
**POST** `/api/attendance/mark`

Mark attendance for a single student (Admin/Teacher only).

**Request Body**
```json
{
  "studentId": 1,
  "attendanceDate": "2024-01-15",
  "status": 0,
  "remarks": "Present"
}
```

**Status Values**
- 0 = Present
- 1 = Absent
- 2 = Late
- 3 = Excused

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Attendance marked successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "studentName": "Alice Johnson",
    "attendanceDate": "2024-01-15",
    "status": "Present",
    "remarks": "Present"
  }
}
```

### Mark Multiple Attendances
**POST** `/api/attendance/mark-multiple`

Mark attendance for multiple students at once (Admin/Teacher only).

**Request Body**
```json
{
  "records": [
    {
      "studentId": 1,
      "status": 0,
      "remarks": "Present"
    },
    {
      "studentId": 2,
      "status": 1,
      "remarks": "Absent"
    }
  ]
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Attendance marked successfully",
  "data": null
}
```

### Get Student Attendance
**GET** `/api/attendance/student/{studentId}`

Retrieve all attendance records for a student.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentName": "Alice Johnson",
      "attendanceDate": "2024-01-15",
      "status": "Present",
      "remarks": "Present"
    }
  ]
}
```

### Get Attendance by Date Range
**GET** `/api/attendance/student/{studentId}/range?startDate=2024-01-01&endDate=2024-01-31`

Retrieve attendance records within a date range.

**Query Parameters**
- `startDate` (required): Start date (YYYY-MM-DD)
- `endDate` (required): End date (YYYY-MM-DD)

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentName": "Alice Johnson",
      "attendanceDate": "2024-01-15",
      "status": "Present",
      "remarks": "Present"
    }
  ]
}
```

### Get Attendance Summary
**GET** `/api/attendance/student/{studentId}/summary?startDate=2024-01-01&endDate=2024-01-31`

Get attendance statistics for a student.

**Query Parameters**
- `startDate` (optional): Start date (YYYY-MM-DD)
- `endDate` (optional): End date (YYYY-MM-DD)

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "studentId": 1,
    "studentName": "Alice Johnson",
    "totalDays": 22,
    "presentDays": 20,
    "absentDays": 2,
    "lateDays": 0,
    "attendancePercentage": 90.91
  }
}
```

---

## Payment Endpoints

### Record Payment
**POST** `/api/payments/record`

Record a payment for a student (Admin only).

**Request Body**
```json
{
  "studentId": 1,
  "amount": 500.00,
  "description": "Monthly tuition fee",
  "transactionId": "TXN20240115001"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Payment recorded successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "studentName": "Alice Johnson",
    "amount": 500.00,
    "status": "Completed",
    "description": "Monthly tuition fee",
    "paymentDate": "2024-01-15",
    "transactionId": "TXN20240115001"
  }
}
```

### Get Payment History
**GET** `/api/payments/student/{studentId}`

Retrieve payment history for a student.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "studentId": 1,
    "studentName": "Alice Johnson",
    "payments": [
      {
        "id": 1,
        "studentId": 1,
        "studentName": "Alice Johnson",
        "amount": 500.00,
        "status": "Completed",
        "description": "Monthly tuition fee",
        "paymentDate": "2024-01-15",
        "transactionId": "TXN20240115001"
      }
    ],
    "totalAmount": 500.00
  }
}
```

### Update Payment Status
**PUT** `/api/payments/{paymentId}/status?status=1`

Update payment status (Admin only).

**Status Values**
- 0 = Pending
- 1 = Completed
- 2 = Failed
- 3 = Refunded

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Payment status updated successfully",
  "data": null
}
```

---

## Grade Endpoints

### Record Grade
**POST** `/api/grades/record`

Record a grade for a student (Admin/Teacher only).

**Request Body**
```json
{
  "studentId": 1,
  "subject": "Mathematics",
  "marks": 85,
  "maxMarks": 100,
  "term": "Term 1"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Grade recorded successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "studentName": "Alice Johnson",
    "subject": "Mathematics",
    "marks": 85,
    "maxMarks": 100,
    "gradeValue": "A",
    "evaluationDate": "2024-01-15",
    "term": "Term 1"
  }
}
```

### Get Student Grades
**GET** `/api/grades/student/{studentId}`

Retrieve all grades for a student.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentName": "Alice Johnson",
      "subject": "Mathematics",
      "marks": 85,
      "maxMarks": 100,
      "gradeValue": "A",
      "evaluationDate": "2024-01-15",
      "term": "Term 1"
    }
  ]
}
```

### Get Grades by Term
**GET** `/api/grades/student/{studentId}/term/{term}`

Retrieve grades for a specific term.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentName": "Alice Johnson",
      "subject": "Mathematics",
      "marks": 85,
      "maxMarks": 100,
      "gradeValue": "A",
      "evaluationDate": "2024-01-15",
      "term": "Term 1"
    }
  ]
}
```

### Update Grade
**PUT** `/api/grades/{gradeId}`

Update a grade (Admin/Teacher only).

**Request Body**
```json
{
  "studentId": 1,
  "subject": "Mathematics",
  "marks": 88,
  "maxMarks": 100,
  "term": "Term 1"
}
```

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Grade updated successfully",
  "data": null
}
```

---

## Report Endpoints

### Generate Report
**POST** `/api/reports/generate?studentId=1&term=Term 1`

Generate a student performance report (Admin/Teacher only).

**Query Parameters**
- `studentId` (required): Student ID
- `term` (required): Term name

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Report generated successfully",
  "data": {
    "id": 1,
    "studentId": 1,
    "studentName": "Alice Johnson",
    "totalAttendance": 22,
    "presentDays": 20,
    "absentDays": 2,
    "attendancePercentage": 90.91,
    "averageGrade": 87.5,
    "term": "Term 1",
    "generatedDate": "2024-01-15",
    "comments": "Excellent academic performance. Good attendance.",
    "grades": [
      {
        "id": 1,
        "studentId": 1,
        "studentName": "Alice Johnson",
        "subject": "Mathematics",
        "marks": 85,
        "maxMarks": 100,
        "gradeValue": "A",
        "evaluationDate": "2024-01-15",
        "term": "Term 1"
      }
    ]
  }
}
```

### Get Student Reports
**GET** `/api/reports/student/{studentId}`

Retrieve all reports for a student.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "studentId": 1,
      "studentName": "Alice Johnson",
      "totalAttendance": 22,
      "presentDays": 20,
      "absentDays": 2,
      "attendancePercentage": 90.91,
      "averageGrade": 87.5,
      "term": "Term 1",
      "generatedDate": "2024-01-15",
      "comments": "Excellent academic performance. Good attendance.",
      "grades": []
    }
  ]
}
```

### Get Specific Report
**GET** `/api/reports/{reportId}`

Retrieve a specific report by ID.

**Response (200 OK)**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": 1,
    "studentId": 1,
    "studentName": "Alice Johnson",
    "totalAttendance": 22,
    "presentDays": 20,
    "absentDays": 2,
    "attendancePercentage": 90.91,
    "averageGrade": 87.5,
    "term": "Term 1",
    "generatedDate": "2024-01-15",
    "comments": "Excellent academic performance. Good attendance.",
    "grades": []
  }
}
```

---

## Error Codes

| Status Code | Description |
|---|---|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

---

## Rate Limiting

API does not currently implement rate limiting. Consider implementing for production.

## Versioning

Current API version: **v1**

---

**Last Updated**: January 2024
