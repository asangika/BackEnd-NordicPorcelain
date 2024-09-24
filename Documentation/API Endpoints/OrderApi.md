## **Orders API Endpoints**

### 1. **Create an Order**

- **Endpoint**: `POST /api/v1/order`
- **Description**: Places a new order.
- **Request Body**:
  ```json
  {
    "userId": "67890",
    "order_items": [
      {
        "order_item_id": "12345",
        "quantity": 2,
        "price": 30
      },
      {
        "order_item_id": "52345",
        "quantity": 2,
        "price": 30
      }
    ],
    "order_date": "2024-08-20T13:00:00Z",
    "total_price": 60.0,
    "shipments": {
      "shipment_id": 12345,
      "shipment_date": "2024-08-20T13:00:00Z",
      "address": {
        "address_id": 1234,
        "unit_number": "sdun",
        "street_number": 23,
        "address_line1": "wall street",
        "address_line2": "Marlin street",
        "city": "Helsinki",
        "postal_code": 00100,
        "country": "Finland"
      }
    },
    "order_status": "pending"
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "order_id": 1234,
      "userId": "67890",
      "order_date": "2024-08-20T13:00:00Z",
      "total_price": 60.0,
      "order_status": "pending"
      "order_items": [
        {
          "order_item_id": "12345",
          "quantity": 2,
          "price": 30
        },
        {
          "order_item_id": "52345",
          "quantity": 2,
          "price": 30
        }
      ]
    }
    ```
  - **400 Bad Request**: Invalid input data.

### 2. **Get All Orders**

- **Endpoint**: `GET /api/v1/orders`
- **Description**: Retrieves a list of orders for a user.
- **Query Parameters** (optional):
  - `?page={pageNumber}&limit={pageSize}`: Pagination options.
  - `?userId={userId}`: Filter orders by user.
  - `?order_status={order_status}`: Filter orders by user.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "userId": "67890",
            "order_items": [
                {
                    "order_item_id": "12345",
                    "quantity": 2,
                    "price": 30
                },
                {
                    "order_item_id": "52345",
                    "quantity": 2,
                    "price": 30
                }
            ],
            "order_date": "2024-08-20T13:00:00Z",
            "total_price": 60.00,
            "shipments":
                {
                    "shipment_id": 12345,
                    "shipment_date": "2024-08-20T13:00:00Z",
                    "address":
                        {
                            "address_id": 1234,
                            "unit_number": "sdun",
                            "street_number": 23,
                            "address_line1": "wall street",
                            "address_line2": "Marlin street",
                            "city": "Helsinki",
                            "postal_code": 00100,
                            "country": "Finland"
                        }
                },
            "order_status": "pending"
        },
        ...
    ]
    ```

### 3. **Get a Single Order by ID**

- **Endpoint**: `GET /api/v1/orders/{orderId}`
- **Description**: Retrieves details of a specific order.
- **Response**:
  - **200 OK**:
    ```json
    {
      "userId": "67890",
      "order_items": [
        {
          "order_item_id": "12345",
          "quantity": 2,
          "price": 30
        },
        {
          "order_item_id": "52345",
          "quantity": 2,
          "price": 30
        }
      ],
      "order_date": "2024-08-20T13:00:00Z",
      "total_price": 60.0,
      "shipments": {
        "shipment_id": 12345,
        "shipment_date": "2024-08-20T13:00:00Z",
        "address": {
          "address_id": 1234,
          "unit_number": "sdun",
          "street_number": 23,
          "address_line1": "wall street",
          "address_line2": "Marlin street",
          "city": "Helsinki",
          "postal_code": 00100,
          "country": "Finland"
        }
      },
      "order_status": "pending"
    }
    ```
  - **404 Not Found**: Order not found.

### 4. **Update an Order**

- **Endpoint**: `PUT /api/v1/orders/{orderId}`
- **Description**: Updates the order (for example reduce order item).
- **Request Body**:
  ```json
  {
    "order_items": [
      {
        "order_item_id": "12345",
        "quantity": 2,
        "price": 30
      }
    ]
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "userId": "67890",
      "order_items": [
        {
          "order_item_id": "12345",
          "quantity": 2,
          "price": 30
        }
      ],
      "order_date": "2024-08-20T13:00:00Z",
      "total_price": 30.0,
      "shipments": {
        "shipment_id": 12345,
        "shipment_date": "2024-08-20T13:00:00Z",
        "address": {
          "address_id": 1234,
          "unit_number": "sdun",
          "street_number": 23,
          "address_line1": "wall street",
          "address_line2": "Marlin street",
          "city": "Helsinki",
          "postal_code": 00100,
          "country": "Finland"
        }
      },
      "order_status": "pending"
    }
    ```
  - **404 Not Found**: Order not found.

### 5. **Delete an Order**

- **Endpoint**: `DELETE /api/v1/orders/{orderId}`
- **Description**: Deletes an order.
- **Response**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Order not found.

---
