# DynamicConfig

A lightweight, high-performance **.NET 8.0 Configuration Library (DLL)** designed for microservices. It fetches configuration settings from a centralized Redis instance at startup and continuously refreshes them in the background using an asynchronous `Timer`.

## ✨ Key Features

* **Zero Network Latency (In-Memory Caching):** Configuration reads are served instantly from an internal local `Dictionary` (RAM) rather than hitting Redis on every request.
* **Encapsulated Type Conversion:** The `GetValue` method automatically converts values to their target runtime types (`int`, `bool`, `string`, etc.) based on the metadata stored in Redis. No generic `<T>` parameters required at runtime.
* **Decoupled Architecture:** The library is completely isolated from ASP.NET Core dependencies. It can be easily integrated into **Web APIs, Worker Services, gRPC, or standalone Console applications**.

---

## 🛠️ Getting Started

### 1. Run Redis via Docker
Ensure a local Redis instance is running before starting the application:
```bash
docker run -d --name dynamic-redis -p 6379:6379 redis