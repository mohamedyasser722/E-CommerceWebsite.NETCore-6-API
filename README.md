# Talabat API

Welcome to the **Talabat API**! This repository contains the backend logic for an e-commerce platform, providing endpoints for user management, products, orders, payments, and more.

## Table of Contents

- [Overview](#overview)
- [Authentication and Authorization](#authentication-and-authorization)
- [Controllers](#controllers)
  - [Account Controller](#account-controller)
  - [Auth Controller](#auth-controller)
  - [Products Controller](#products-controller)
  - [Orders Controller](#orders-controller)
  - [Payments Controller](#payments-controller)
  - [Users Controller](#users-controller)
- [Permissions](#permissions)
- [Database Schema](#database-schema)
- [How to Run the Project](#how-to-run-the-project)

---

## Overview

The **Talabat API** is a backend system built using ASP.NET Core to support the functionality of managing an e-commerce platform. It includes user management, product listings, order processing, and Stripe-based payment handling.

### Core Features:
- User Authentication & Authorization
- Product Management
- Order Creation & Tracking
- Payment Integration via Stripe
- Role-based access control

## Authentication and Authorization

This API uses **JWT tokens** for authentication and role-based authorization to ensure secure access to protected resources. Make sure to include a valid JWT token in your requests' `Authorization` header.

## Controllers

### Account Controller

The `AccountController` handles user account-related operations such as fetching and updating user profile information and changing passwords.

- `GET /me`: Fetch current user's profile information.
- `PUT /me/info`: Update current user's profile.
- `PUT /me/change-password`: Change the current user's password.

### Auth Controller

The `AuthController` is responsible for handling user authentication actions such as login, registration, and token management.

- `POST /auth`: Log in with email and password.
- `POST /auth/refresh`: Refresh an expired JWT token.
- `POST /auth/register`: Register a new user.

### Products Controller

The `ProductsController` manages operations related to listing, retrieving, adding, updating, and deleting products.

- `GET /api/products`: Fetch all products.
- `GET /api/products/{id}`: Fetch product details by ID.
- `POST /api/products`: Create a new product.
- `PUT /api/products/{id}`: Update an existing product.
- `DELETE /api/products/{id}`: Delete a product.

### Orders Controller

The `OrdersController` handles order creation, tracking, and management.

- `GET /api/orders`: Fetch all orders.
- `GET /api/orders/{id}`: Fetch order details by ID.
- `POST /api/orders`: Create a new order.
- `PUT /api/orders/{id}`: Update an existing order.
- `DELETE /api/orders/{id}`: Cancel an order.

### Payments Controller

The `PaymentsController` integrates Stripe to manage payments for orders.

- `POST /api/payments`: Process a payment using Stripe.
- `GET /api/payments/history`: Get the payment history for a user.
  
### Users Controller

The `UsersController` manages user-related tasks such as retrieving user information and updating user statuses.

- `GET /api/users`: Fetch all users.
- `GET /api/users/{id}`: Get details of a specific user.
- `POST /api/users`: Create a new user.
- `PUT /api/users/{id}`: Update an existing user.
- `PUT /api/users/{id}/toggle-status`: Toggle the active status of a user.
  
## Permissions

Permissions are enforced at the controller and action level to restrict access based on the user's role and assigned permissions. These permissions are set in attributes such as `[HasPermission(Permissions.AddProduct)]`.

## Database Schema

The **Talabat API** database schema is designed around core entities such as `Product`, `Order`, `Payment`, and `ApplicationUser`. Below is a high-level overview of the tables and relationships.

### Key Entities and Relationships:

- **ApplicationUser**: Represents the system's users, extending ASP.NET Core Identity features. Each user can have many `Orders` and `Payments`.
  
  - One-to-Many: `ApplicationUser` ↔ `Order`
  - One-to-Many: `ApplicationUser` ↔ `Payment`

- **Product**: Contains product information such as name, price, and stock. A product can be associated with many `Orders`.
  
  - One-to-Many: `Product` ↔ `Order`

- **Order**: Represents customer purchases. Each order contains references to `Products` and is linked to an `ApplicationUser`.
  
  - One-to-Many: `Order` ↔ `Product`
  - Many-to-One: `Order` ↔ `ApplicationUser`

- **Payment**: Represents a payment made for an order. Each payment is associated with an `Order` and an `ApplicationUser`.

  - One-to-One: `Payment` ↔ `Order`
  - Many-to-One: `Payment` ↔ `ApplicationUser`

### Database Diagram

The schema can be visualized as follows:

```plaintext
ApplicationUser
    ↳ Order (1-N)
    ↳ Payment (1-N)

Product
    ↳ Order (1-N)

Order
    ↳ Product (1-N)
    ↳ Payment (1-1)
```

Each arrow (↳) represents a one-to-many (1-N) or one-to-one (1-1) relationship. For example, one `Order` can contain many `Products`, and one `Payment` is linked to one `Order`.

## How to Run the Project

To run the project locally:

1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/Talabat.Api.git
   ```

2. Just run the project
   ```bash
  when you run the project everything will be set for you, including creating the database and seeding it with initial data
   ```

---
