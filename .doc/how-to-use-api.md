Sua versão já está bastante direta e limpa, mas alguns pequenos ajustes podem deixar o documento **mais consistente e fácil de seguir**, especialmente para novos desenvolvedores/testadores:

---

# API Test Documentation — `Sales`

### 🟢 STEP 1 — Create a Sale

**Endpoint:**
`POST /api/Sales`

**Headers:**

```
Content-Type: application/json
Accept: application/json
```

**Body (copy & paste):**

```json
{
  "saleNumber": "SALE-2026-0001",
  "saleDate": "2026-01-18T20:48:37.250Z",
  "customer": "John Doe",
  "branch": "Main Branch",
  "items": [
    {
      "product": "Notebook Dell XPS 13",
      "quantity": 3,
      "unitPrice": 7500.00
    },
    {
      "product": "Mouse Logitech MX Master 3",
      "quantity": 5,
      "unitPrice": 450.00
    },
    {
      "product": "Mechanical Keyboard Keychron K6",
      "quantity": 10,
      "unitPrice": 650.00
    }
  ]
}
```

**Expected Response:**

* Status: `201 Created`
* Save the `saleId` and each `itemId` for later requests.

---

### 🟢 STEP 2 — List All Sales

**Endpoint:**
`GET /api/Sales`

**Headers:**

```
Accept: application/json
```

**Expected Response:**

* Status: `200 OK`
* The newly created sale appears in the list with each item having a unique `id`.

---

### 🟢 STEP 3 — Get Sale by ID

**Endpoint:**
`GET /api/Sales/{saleId}`

**Example:**

```
GET /api/Sales/e367bf34-3b37-4cdb-84f4-c1fa62ee851c
```

**Headers:**

```
Accept: application/json
```

**Expected Response:**

* Status: `200 OK`
* Full sale details, including all item IDs.

---

### 🟡 STEP 4 — Update a Sale (PUT)

**Important:**

* Item IDs MUST match the ones returned in the GET request.
* Items not included will be removed.

**Endpoint:**
`PUT /api/Sales/{saleId}`

**Headers:**

```
Content-Type: application/json
Accept: application/json
```

**Body (example):**

```json
{
  "customer": "John Doe Updated",
  "branch": "Main Branch",
  "items": [
    {
      "product": "Notebook Dell XPS 13",
      "quantity": 2,
      "unitPrice": 7400.00
    },
    {
      "product": "Mechanical Keyboard Keychron K6",
      "quantity": 8,
      "unitPrice": 620.00
    }
  ]
}
```

---

### 5️⃣ Cancel a Sale

**Endpoint:**
`POST /api/Sales/{saleId}/cancel`

**Headers:**

```
Accept: application/json
```

---

### 6️⃣ Cancel a Sale Item

**Endpoint:**
`POST /api/Sales/{saleId}/items/{itemId}/cancel`

**Headers:**

```
Accept: application/json
```

---

### 7️⃣ Delete a Sale

**Endpoint:**
`DELETE /api/Sales/{saleId}`

**Headers:**

```
Accept: application/json
```



