# Product Service API

A RESTful API service built with ASP.NET Core that manages products, their variants, images, and categories.

## Features

- Product management (CRUD operations)
- Product variant management (size, color, stock)
- Product image management
- Category management
- Pagination and filtering support

## Project Structure

```
ProductService/
├── Controllers/          # API controllers
├── Data/                 # Database context and migrations
├── Dtos/                 # Data Transfer Objects
│   ├── Models/           # Shared DTO models
│   ├── Requests/         # Request DTOs
│   └── Responses/        # Response DTOs
├── Models/                 # Entity models
├── Services/             # Business logic services
│   ├── Interfaces/       # Service interfaces
│   └── Implementations/  # Service implementations
└── Program.cs            # Application entry point
```

## Getting Started

### Prerequisites

- .NET 7.0 SDK or later
- PostgreSQL database
- Docker

### Installation

1. Clone the repository
2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Configure the database connection in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ProductServiceDb;User Id=your_user;Password=your_password;"
     }
   }
   ```

4. Run database migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

### Using Docker

1. Build and run the Docker container:
   ```bash
   docker build -t product-service .
   docker run -p 5000:80 product-service
   ```

## API Endpoints

### Products

- GET `/Product` - Get all products
- GET `/Product/{id}` - Get a specific product
- POST `/Product` - Create a new product
- PUT `/Product/{id}` - Update a product
- DELETE `/Product/{id}` - Delete a product

### Product Variants

- GET `/Product/{productId}/Variants` - Get product variants
- POST `/Product/{productId}/Variants` - Add a variant
- PUT `/Product/{productId}/Variants/{variantId}` - Update a variant
- DELETE `/Product/{productId}/Variants/{variantId}` - Delete a variant

### Product Images

- GET `/Product/{productId}/Images` - Get product images
- POST `/Product/{productId}/Images` - Add an image
- PUT `/Product/{productId}/Images/{imageId}` - Update an image
- DELETE `/Product/{productId}/Images/{imageId}` - Delete an image

## DTOs

### Request DTOs

- `CreateProductRequest` - For creating new products
- `UpdateProductRequest` - For updating products
- `GetProductRequest` - For filtering products
- `CreateProductImageRequest` - For adding product images
- `CreateProductVariantRequest` - For adding product variants

### Response DTOs

- `ProductResponse` - Product details
- `ProductImageResponse` - Product image details
- `GetProductResponse` - Filtered product list

## Error Handling

The API uses standard HTTP status codes and returns error responses in the following format:

```json
{
    "type": "string",
    "title": "string",
    "status": integer,
    "errors": {
        "fieldName": ["error message"]
    },
    "traceId": "string"
}
```

## Testing

The API includes Swagger/OpenAPI documentation that can be accessed at `/swagger` when running the application.

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.