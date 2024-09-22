Here's the detailed description for the **Talabat E-Commerce Web API** project based on the controllers, entities, and architecture you've provided:

---

## **Talabat E-Commerce Web API Project**

**Overview:**
The **Talabat E-Commerce Web API** is designed to manage the core functionalities of an online store, including user authentication, basket handling, product management, order processing, and payment integration via Stripe. It follows a clean, layered architecture utilizing the **Repository Pattern** with **Unit of Work**, ensuring separation of concerns and scalability.

The project is divided into four key layers:
1. **APIs Layer** (`Talabat.APIs`): Handles incoming HTTP requests, processes them, and returns responses.
2. **Core Layer** (`Talabat.Core`): Contains the domain entities and service contracts.
3. **Repository Layer** (`Talabat.Repository`): Handles data access and CRUD operations.
4. **Service Layer** (`Talabat.Service`): Contains business logic and integrates with third-party services like Stripe.

---

### **Controllers Overview:**

1. **AccountController**:
   - Handles user authentication and registration using **ASP.NET Identity**.
   - Key endpoints:
     - `POST /register`: Registers a new user and generates a JWT token.
     - `POST /login`: Authenticates an existing user and returns a JWT token.
     - `GET /GetCurrentUser`: Fetches the current logged-in user's details.
     - `GET /GetCurrentUserAddress`: Retrieves the user's saved address.
     - `PUT /UpdateUserAddress`: Updates the user's address.
     - `GET /emailExists`: Checks if an email is already registered.

2. **BasketsController**:
   - Manages the shopping basket (cart) for users.
   - Key endpoints:
     - `GET /{BasketId}`: Fetches an existing basket by its ID or creates a new one if not found.
     - `POST /`: Updates or creates a new basket based on the provided details.
     - `DELETE /{BasketId}`: Deletes a specific basket.

3. **ErrorsController**:
   - Handles application-level errors and returns appropriate error messages with HTTP status codes.
   - Key endpoint:
     - `GET /errors/{code}`: Returns a custom error message based on the provided HTTP status code.

4. **OrdersController**:
   - Manages order placement and retrieval for users.
   - Key endpoints:
     - `POST /`: Creates a new order from the user's basket.
     - `GET /`: Retrieves all orders for the logged-in user.
     - `GET /{id}`: Fetches a specific order by its ID.
     - `GET /DeliveryMethods`: Returns available delivery methods.

5. **PaymentController**:
   - Handles payment processing via Stripe.
   - Key endpoints:
     - `POST /basketId`: Creates or updates a payment intent for the user's basket.
     - `POST /webhook`: Listens to Stripe webhook events (e.g., payment success or failure) to update payment statuses.

6. **ProductController**:
   - Manages products in the store, allowing for retrieval by category, brand, and product ID.
   - Key endpoints:
     - `GET /`: Fetches a paginated list of all products, with filtering options.
     - `GET /id`: Retrieves a specific product by ID.
     - `GET /Categories`: Returns a list of product categories.
     - `GET /Brands`: Returns a list of product brands.

---

### **Entities Overview:**

1. **AppUser**: Represents a user in the system with properties like `DisplayName`, `Email`, `PhoneNumber`, and `Address`.

2. **CustomerBasket**: Represents a user's shopping basket containing items. Each basket has:
   - `Id`: Basket identifier.
   - `Items`: List of items in the basket.
   - `PaymentIntentId`, `ClientSecret`: Fields related to payment processing with Stripe.

3. **BasketItem**: Represents individual items in the shopping basket, including fields like `Name`, `Brand`, `Category`, `Price`, and `Quantity`.

4. **Product**: Represents an item in the store with details like `Name`, `PictureUrl`, `Price`, `Description`, and its relationships to `ProductCategory` and `ProductBrand`.

5. **ProductCategory**: Represents product categories like Electronics, Clothing, etc.

6. **ProductBrand**: Represents product brands like Apple, Samsung, etc.

7. **Order**: Represents a user's order with properties like `BuyerEmail`, `ShipToAddress`, and `OrderItems`.

8. **DeliveryMethod**: Represents various delivery options with details like `Price` and `DeliveryTime`.

---

### **Architecture and Design Patterns:**

- **Repository Pattern**: Each entity has its own repository for data access, ensuring separation between business logic and database interaction. The repositories are managed by the `IUnitOfWork` for atomic transactions.
  
- **Unit of Work**: This design pattern is used to manage database transactions and ensure data consistency. It groups all operations within a transaction, and if one fails, the entire transaction is rolled back.

- **AutoMapper**: Used to map between domain entities and DTOs (Data Transfer Objects) to ensure that only necessary data is exposed through the API.

- **ASP.NET Identity**: Provides user management, including authentication and authorization with JWT tokens.

---

### **Third-Party Integrations:**

- **Stripe**: Integrated for payment processing. The API handles creating and updating payment intents for user baskets and listens to Stripe's webhook events to track payment success or failure.

---

### **Authentication & Authorization:**
- The API uses **JWT-based authentication** where users need to register or log in to obtain a token. Some actions, like creating orders or accessing user-specific data, require authorization, which is handled using **ASP.NET Core's `[Authorize]`** attribute.

---
