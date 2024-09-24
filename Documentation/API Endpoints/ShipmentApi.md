## Shipments API Endpoints

### 1. Create a Shipment

- **Endpoint**: `POST /api/v1/shipment`
- **Description**: Creates a new shipment.
- **Request Body**:

  ```json
  {
    "order_id": 1234,
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
  }
  ```

  - **Response**:
  - **200 OK**:

    ```json
    {
      "shipment_id": 5678,
      "order_id": 1234,
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
    }
    ```

- **400 Bad Request**: Invalid input data.

### 2. Update a Shipment

- **Endpoint**: `PUT /api/v1/shipments/{shipment_id}`
- **Description**: Updates an existing shipment.
- **Request Body**:

  ```json
  {
    "shipment_date": "2024-08-21T13:00:00Z",
    "address": {
      "address_id": 1234,
      "unit_number": "new_unit",
      "street_number": 25,
      "address_line1": "new street",
      "address_line2": "New Marlin street",
      "city": "Helsinki",
      "postal_code": 00200,
      "country": "Finland"
    }
  }
  ```

  - **Response**:
  - **200 OK**:

    ```json
    {
      "shipment_id": 5678,
      "order_id": 1234,
      "shipment_date": "2024-08-21T13:00:00Z",
      "address": {
        "address_id": 1234,
        "unit_number": "new_unit",
        "street_number": 25,
        "address_line1": "new street",
        "address_line2": "New Marlin street",
        "city": "Helsinki",
        "postal_code": 00200,
        "country": "Finland"
      }
    }
    ```

    - **400 Bad Request**: Invalid input data.
    - **404 Not Found**: Shipment not found.

### 3. Delete a Shipment

- **Endpoint**: `DELETE /api/v1/shipments/{shipment_id}`
- **Description**: Deletes a shipment.
- **Responses**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: Shipment not found.

### 4. Get Shipment Details

- **Endpoint**: `GET /api/v1/shipments/{shipment_id}`
- **Description**: Retrieves details of a specific shipment.
- **Responses**:
  - **200 OK**:
    ```json
    {
      "shipment_id": 5678,
      "order_id": 1234,
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
    }
    ```
  - **404 Not Found**: Shipment not found.

### 5. List Shipments

- **Endpoint**: `GET /api/v1/shipments`
- **Description**: Retrieves a list of all shipments.
- **Responses**:
  - **200 OK**:
    ```json
    [
      {
        "shipment_id": 5678,
        "order_id": 1234,
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
      {
        "shipment_id": 5679,
        "order_id": 1235,
        "shipment_date": "2024-08-21T13:00:00Z",
        "address": {
          "address_id": 1235,
          "unit_number": "another_unit",
          "street_number": 25,
          "address_line1": "another street",
          "address_line2": "Another Marlin street",
          "city": "Helsinki",
          "postal_code": 00200,
          "country": "Finland"
        }
      }
    ]
    ```

---
