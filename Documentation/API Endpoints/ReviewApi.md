## **Reviews API Endpoints**

### 1. **Create a Review**

- **Endpoint**: `POST /api/v1/review`
- **Description**: Adds a new review to a product.
- **Authentication**: yes
- **Request Body**:
  ```json
  {
    "productId": "12345",
    "userId": "67890",
    "review_date": "2024-08-20T13:00:00Z",
    "rating": 5,
    "review": "This product is amazing!",
    "anonymize": true
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "review_id": "abc123",
      "productId": "12345",
      "userId": "67890",
      "review_date": "2024-08-20T13:00:00Z",
      "rating": 5,
      "review": "This product is amazing!",
      "anonymize": true
    }
    ```
  - **400 Bad Request**: Invalid input data.

### 2. **Get All Reviews**

- **Endpoint**: `GET /api/v1/reviews`
- **Description**: Retrieves a list of all reviews.
- **Query Parameters** (optional):
  - `?page={pageNumber}&limit={pageSize}`: Pagination options.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "review_id": "abc123",
            "productId": "12345",
            "userId": "67890",
            "review_date": "2024-08-20T13:00:00Z",
            "rating": 5,
            "review": "This product is amazing!"
        },
        ...
    ]
    ```

### 3. **Get All Reviews of a Specific Product**

- **Endpoint**: `GET /api/v1/reviews/products/{productId}`
- **Description**: Retrieves reviews for a specific product.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "review_id": "abc123",
            "productId": "12345",
            "userId": "67890",
            "review_date": "2024-08-20T13:00:00Z",
            "rating": 5,
            "review": "This product is amazing!"
        },
        ...
    ]
    ```

### 4. **Get All Reviews of a Specific User**

- **Endpoint**: `GET /api/v1/reviews/users/{userId}`
- **Description**: Retrieves reviews made by a specific user.
- **Authentication**: yes
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "review_id": "abc123",
            "productId": "12345",
            "userId": "67890",
            "review_date": "2024-08-20T13:00:00Z",
            "rating": 5,
            "review": "This product is amazing!"
        },
        ...
    ]
    ```

### 5. **Get a Single Review by ID**

- **Endpoint**: `GET /api/v1/reviews/{reviewId}`
- **Description**: Retrieves details of a specific review.
- **Response**:
  - **200 OK**:
    ```json
    {
      "review_id": "abc123",
      "productId": "12345",
      "userId": "67890",
      "review_date": "2024-08-20T13:00:00Z",
      "rating": 5,
      "review": "This product is amazing!"
    }
    ```
  - **404 Not Found**: Review not found.

### 6. **Update a Review**

- **Endpoint**: `PUT /api/v1/reviews/{reviewId}`
- **Description**: Updates the details of an existing review.
- **Authentication**: yes
- **Request Body**:
  ```json
  {
    "rating": 4,
    "review": "This product is great!"
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "review_id": "abc123",
      "productId": "12345",
      "userId": "67890",
      "review_date": "2024-08-20T13:00:00Z",
      "rating": 4,
      "review": "This product is great!"
    }
    ```
  - **404 Not Found**: Review not found.

### 7. **Delete a Review**

- **Endpoint**: `DELETE /api/v1/reviews/{reviewId}`
- **Description**: Deletes a review.
- **Authentication**: yes
- **Response**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Review not found.

---
