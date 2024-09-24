## Payments API Endpoints

### **1. Create a Payment**

- **Endpoint**: `POST /api/v1/payments`
- **Description**: Initiates a new payment for an order.
- **Request Body**:
  ```json
  {
    "orderId": 12345,
    "user_id": 23453,
    "payment_date": "2024-08-20T13:00:00Z",
    "amount": 30.0,
    "payment_method": {
      "payment_method_id": 1234,
      "payment_type": "credit card",
      "provider": "nordea",
      "card_number": "4111111111111111",
      "expiry_date": "12/24",
      "isDefault": true
    }
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "orderId": 12345,
      "user_id": 23453,
      "status": "processing",
      "payment_date": "2024-08-20T13:00:00Z",
      "amount": 30.0,
      "payment_method": {
        "payment_method_id": 1234,
        "payment_type": "credit card",
        "provider": "nordea",
        "card_number": "4111111111111111",
        "expiry_date": "12/24",
        "isDefault": true
      }
    }
    ```
  - **400 Bad Request**: Invalid input data (e.g., missing fields, invalid card details).

### **2. Get Payment Details**

- **Endpoint**: `GET /api/v1/payments/{paymentId}`
- **Description**: Retrieves details of a specific payment.
- **Response**:
  - **200 OK**:
    ```json
    {
      "orderId": 12345,
      "user_id": 23453,
      "status": "completed",
      "payment_date": "2024-08-20T13:00:00Z",
      "amount": 30.0,
      "payment_method": {
        "payment_method_id": 1234,
        "payment_type": "credit card"
      }
    }
    ```
  - **404 Not Found**: Payment not found.

### **3. Get All Payments**

- **Endpoint**: `GET /api/v1/payments`
- **Description**: Retrieves a list of all payments.
- **Authorization**: yes
- **Query Parameters** (optional):
  - `?orderId={orderId}`: Filter payments by order ID.
  - `?userId={userId}`: Filter payments by user ID.
  - `?page={pageNumber}&limit={pageSize}`: Pagination options.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "orderId": 12345,
            "user_id": 23453,
            "status": "completed",
            "payment_date": "2024-08-20T13:00:00Z",
            "amount": 30.0,
            "payment_method":
                {
                    "payment_method_id": 1234,
                    "payment_type": "credit card"
                }
        },
        ...
    ]
    ```

### **4. Update Payment Status**

- **Endpoint**: `PUT /api/v1/payments/{paymentId}`
- **Description**: Updates the status of a payment (e.g., from "processing" to "completed").
- **Authorization**: yes
- **Request Body**:
  ```json
  {
    "status": "completed"
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "orderId": 12345,
      "user_id": 23453,
      "status": "completed",
      "payment_date": "2024-08-20T13:00:00Z",
      "amount": 30.0,
      "payment_method": {
        "payment_method_id": 1234,
        "payment_type": "credit card",
        "provider": "nordea",
        "card_number": "4111111111111111",
        "expiry_date": "12/24",
        "isDefault": true
      }
    }
    ```
  - **404 Not Found**: Payment not found.

### **5. Delete a Payment**

- **Endpoint**: `DELETE /api/v1/payments/{paymentId}`
- **Description**: Deletes a payment record.
- **Authorization**: yes
- **Response**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Payment not found.

---
