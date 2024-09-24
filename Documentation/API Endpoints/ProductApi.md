## **Products API Endpoints**

### 1. **Create a Product**

- **Endpoint**: `POST /api/v1/product`
- **Description**: Adds a new product to the inventory.
- **Authorization**: yes
- **Request Body**:
  ```json
  {
    "name": "Product Name",
    "description": "Product Description",
    "price": 29.99,
    "category": {
      "category_id": 12376
    },
    "stock": 100,
    "brand_name": "footwear",
    "product_images": {
      "image_id": 12345,
      "imageUrl": "http://example.com/image.jpg",
      "isPrimary": true,
      "image_text": "image text"
    },
    "product_sizes": {
      "size_id": 12345,
      "size_value": "L",
      "quantity": 5
    },
    "product_colors": {
      "color_id": 234567,
      "color_name": "red"
    }
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "product_id": "12345",
      "name": "Product Name",
      "description": "Product Description",
      "price": 29.99,
      "category": {
        "category_id": 12376
      },
      "stock": 100,
      "brand_name": "footwear",
      "product_images": {
        "image_id": 12456,
        "imageUrl": "http://example.com/image.jpg",
        "isPrimary": true,
        "image_text": "image text"
      },
      "product_sizes": {
        "size_id": 12345,
        "size_value": "L",
        "quantity": 5
      },
      "product_colors": {
        "color_id": 234567,
        "color_name": "red"
      }
    }
    ```
  - **400 Bad Request**: Invalid input data.

### 2. **Get All Products**

- **Endpoint**: `GET /api/v1/products`
- **Description**: Retrieves a list of all products.
- **Query Parameters** (optional):
  - `?category={categoryName}`: Filter products by category.
  - `?page={pageNumber}&limit={pageSize}`: Pagination options.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "product_id": "12345",
            "name": "Product Name",
            "description": "Product Description",
            "price": 29.99,
            "category":
                {
                    "category_id": 12345
                },
            "stock": 100,
            "brand_name": "footwear",
            "product_images":
                {
                    "image_id": 12456,
                    "imageUrl": "http://example.com/image.jpg",
                    "isPrimary": true,
                    "image_text": "image text"
                },
            "product_sizes":
                {
                    "size_id": 12345,
                    "size_value": "L",
                    "quantity": 5
                },
            "product_colors":
                {
                    "color_id": 12346,
                    "color_name": "red"
                }
        },
        ...
    ]
    ```

### 3. **Get a Single Product by ID**

- **Endpoint**: `GET /api/v1/products/{productId}`
- **Description**: Retrieves details of a specific product.
- **Response**:
  - **200 OK**:
    ```json
    {
      "product_id": "12345",
      "name": "Product Name",
      "description": "Product Description",
      "price": 29.99,
      "category": {
        "category_id": 12345
      },
      "stock": 100,
      "brand_name": "footwear",
      "product_images": {
        "image_id": 12456,
        "imageUrl": "http://example.com/image.jpg",
        "isPrimary": true,
        "image_text": "image text"
      },
      "product_sizes": {
        "size_id": 12345,
        "size_value": "L",
        "quantity": 5
      },
      "product_colors": {
        "color_id": 12345,
        "color_name": "red"
      }
    }
    ```
  - **404 Not Found**: Product not found.

### 4. **Update a Product**

- **Endpoint**: `PUT /api/v1/products/{productId}`
- **Description**: Updates the details of an existing product.
- **Authentication**: yes
- **Request Body**:
  ```json
  {
    "name": "Updated Product Name",
    "description": "Updated Product Description",
    "price": 34.99,
    "stock": 80
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "product_id": "12345",
      "name": "Updated Product Name",
      "description": "Updated Product Description",
      "price": 34.99,
      "category": {
        "category_id": 12345
      },
      "stock": 80,
      "brand_name": "footwear",
      "product_images": {
        "image_id": 12456,
        "imageUrl": "http://example.com/image.jpg",
        "isPrimary": true,
        "image_text": "image text"
      },
      "product_sizes": {
        "size_id": 12345,
        "size_value": "L",
        "quantity": 5
      },
      "product_colors": {
        "color_id": 12345,
        "color_name": "red"
      }
    }
    ```
  - **404 Not Found**: Product not found.

### 5. **Delete a Product**

- **Endpoint**: `DELETE /api/v1/products/{productId}`
- **Description**: Deletes a product from the inventory.
- **Authentication**: yes
- **Response**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Product not found.

### 6. **Get Most Purchased Products**

- **Endpoint**: `GET /api/v1/products/most-purchased`
- **Description**: Retrieves the most purchased products.
- **Query Parameters**:
  - `?limit={x}`: The number of top purchased products to retrieve.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
        "product_id": "12345",
        "name": "Product Name",
        "description": "Product Description",
        "price": 29.99,
        "category":
            {
                "category_id": 12345
            },
        "stock": 100,
        "brand_name": "footwear",
        "product_images":
            {
                "image_id": 12456,
                "imageUrl": "http://example.com/image.jpg",
                "isPrimary": true,
                "image_text": "image text"
            },
        "product_sizes":
            {
                "size_id": 12345,
                "size_value": "L",
                "quantity": 5
            },
        "product_colors":
            {
                "color_id": 12345,
                "color_name": "red"
            }
    },
        ...
    ]
    ```

---
