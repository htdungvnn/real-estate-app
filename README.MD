# Real Estate App Architecture

## 1. Architecture: Microservices

### Services Breakdown

- **User Service:**

  - Manage user registration, authentication, and profiles.
  - Identity Service integrated with **Duende IdentityServer**.
  - Uses **SQL Server** for user data and **EF Core** for ORM.

- **Property Service:**

  - Handles property listings, details, images, and metadata.
  - **SQL Server** for structured data.
  - **Elasticsearch** for optimized search queries.

- **Search Service:**

  - Utilizes **Elasticsearch** for fast property search.
  - Indexing operations using data from **SQL Server** and **MongoDB**.

- **Action Tracking Service:**

  - Tracks user interactions like favorites, views, and clicks.
  - Stores data in **MongoDB** for flexibility and scalability.

- **Chat Service:**

  - Powered by **SignalR** for real-time communication.
  - Uses **Redis** for managing real-time connection states and caching.

- **Notification Service:**

  - Sends emails and in-app notifications.
  - Uses **MailKit** for email and integrates with **SignalR** for push notifications.

- **Cache Service:**
  - Central caching layer using **Redis**.
  - Optimizes performance for paginated results and frequent queries.

---

## 2. Database Setup

- **SQL Server:**

  - Core structured data (users, properties, transactions).
  - Utilized with **Entity Framework Core** for business logic.

- **Redis:**

  - Cache frequently accessed data (e.g., paginated results).
  - Used for connection management in **SignalR**.

- **MongoDB:**

  - User actions (e.g., likes, views, and history).
  - Schema-less storage for flexibility.

- **Elasticsearch:**
  - Full-text search and filtering for properties.
  - Integration with **Logstash** for log processing (optional).

---

## 3. Back-End Implementation

- **Framework:** ASP.NET Core 9.0

- **Communication:**

  - **gRPC** or **REST APIs** for service-to-service communication.
  - Event-driven architecture using **Kafka** for inter-service messaging.

- **Business Logic:**

  - Centralized in specific services using **EF Core** for database interactions.

- **Authentication:**

  - **Duende IdentityServer** for OAuth2/OpenID Connect.
  - Secure token issuance for API access.

- **Logging and Monitoring:**
  - **Serilog** for structured logging.
  - **Prometheus** and **Grafana** for monitoring.

---

## 4. Front-End Implementation

- **Framework:** Angular 19

- **Styling:**

  - **Tailwind CSS** for custom designs.
  - **Angular Material** for pre-built UI components.

- **State Management:**

  - **NgRx** for handling state in Angular.

- **Real-Time Features:**

  - Integrates with **SignalR** for chat and notifications.

- **Routing:**
  - Lazy-loaded modules for faster page loads.

---

## 5. CI/CD Pipeline

- **Continuous Integration (CI):**

  - **GitHub Actions** for building, testing, and packaging services as Docker containers.

- **Continuous Deployment (CD):**

  - **Kubernetes (K8s)** for container orchestration.
  - **Helm charts** for managing deployments.
  - Rollback mechanisms in **K8s** for stability.

- **Docker Setup:**
  - Each service containerized with a shared network.
  - Uses multi-stage Dockerfiles for efficient builds.

---

## 6. Event Streaming

- **Kafka:**
  - Handles asynchronous communication between microservices.
  - Topics for:
    - User actions (e.g., searches, clicks).
    - Notifications (email, chat updates).
    - Property updates (e.g., new listings).

---

## 7. External Integrations

- **Payment Gateway:**

  - **Stripe** for subscription-based payments or premium property listings.
  - Supports multiple currencies and fraud detection.

- **Map Services:**
  - **Google Maps API** for property location visualization and geocoding.

---

## 8. Development Tools and Libraries

- **ASP.NET Libraries:**

  - **EF Core** for data access.
  - **Duende IdentityServer** for authentication.
  - **SignalR** for real-time chat.

- **Angular Libraries:**

  - **Angular Material** for UI components.
  - **NgRx** for state management.

- **Docker:**

  - For containerizing services.

- **Redis:**

  - For caching and real-time state.

- **MailKit:**
  - For email notifications.

---

## 9. Deployment and Hosting

- **Cloud Provider:**

  - **AWS** for Elastic Kubernetes Service (EKS), RDS, and S3 for storage.

- **Load Balancer:**

  - Use **NGINX** or AWS's **Application Load Balancer**.

- **SSL/TLS:**
  - Secure communication using **Let’s Encrypt** or cloud-provided SSL certificates.

---

## 10. High-Level Service Communication Diagram

1. User registers via Angular UI (**Identity Service** handles authentication).
2. Property data fetched via **Property Service** (data in SQL Server/Elasticsearch).
3. User actions stored in **MongoDB** (via Action Tracking Service).
4. Real-time notifications and chat updates through **SignalR** and **Kafka**.
5. Emails dispatched by **Mail Service** using **MailKit**.
6. **CI/CD** ensures rapid and reliable deployment of updates.
