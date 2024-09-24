## Categories API Endpoints

### **1. Create a Category**

- **Endpoint**: `POST /api/v1/category`
- **Description**: Creates a new product category.
- **Request Body**:
  ```json
  {
    "name": "Men's clothing",
    "parent_category_id": 12456
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "id": 12345,
      "name": "Men's clothing",
      "parent_category_id": 12456
    }
    ```
  - **400 Bad Request**: Invalid input data.

### **2. Get All Categories**

- **Endpoint**: `GET /api/v1/categories`
- **Description**: Retrieves a list of all product categories.
- **Query Parameters** (optional):
  - `?parent_category_id={parent_category_id}`: Filter categories by parent category.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "id": 12345,
            "name": "Men's clothing",
            "parent_category_id": 12456
        },
        {
            "id": 12346,
            "name": "Women's clothing",
            "parent_category_id": 12458
        },
        ...
    ]
    ```

### **3. Get Category by ID**

- **Endpoint**: `GET /api/v1/categories/{categoryId}`
- **Description**: Retrieves details of a specific category by its ID.
- **Response**:
  - **200 OK**:
    ```json
    {
      "id": 12456,
      "name": "Men's clothing",
      "parent_category_id": 12456
    }
    ```
  - **404 Not Found**: Category not found.

### **4. Update a Category**

- **Endpoint**: `PUT /api/v1/categories/{categoryId}`
- **Description**: Updates the details of an existing category.
- **Request Body**:
  ```json
  {
    "name": "Updated Men's clothing",
    "parent_category_id": 12456
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "id": 12345,
      "name": "Updated Men's clothing",
      "parent_category_id": 12456
    }
    ```
  - **404 Not Found**: Category not found.

### **5. Delete a Category**

- **Endpoint**: `DELETE /api/v1/categories/{categoryId}`
- **Description**: Deletes a category by its ID.
- **Response**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Category not found.

---
