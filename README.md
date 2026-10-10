# 🧠 Thinking in Design Patterns

**Understand the problem. Explore the design. Write better C# code.**

Welcome to **ThinkingInDesignPatterns**, a collection of design pattern explanations and C# examples focused on the reasoning behind software design decisions.

The goal is not to memorize diagrams or use patterns everywhere. It is to understand **why a pattern exists, what problem it solves, how it works, and when the extra abstraction is worth it**.

This repository explores classic object-oriented design patterns alongside concepts you may recognize from modern **C# and .NET** development.

> **Start with the problem, not the pattern.** A useful pattern makes a design easier to change, understand, or test. An unnecessary pattern simply adds complexity.

---

## 📚 How This Repository Is Organized

**The `master` branch is the entry point and learning index.** Each pattern is explored in its **own Git branch**, with a dedicated README and examples.

Choose a pattern below to open its branch directly. You do **not** need to merge the pattern branches into `master` to explore them.

## 🧭 Pattern Library

### 🏗️ Creational Patterns — *How should objects be created?*

Creational patterns help manage object construction, selection, and reuse without tying client code to unnecessary creation details.

| Pattern                      | What you'll learn                                                                                  | Branch                                                                                                       |
| ---------------------------- | -------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------ |
| **Factory / Factory Method** | Centralizing creation decisions and reducing dependencies on concrete implementations.             | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/CreationalPatterns.FactoryMethod) |
| **Builder**                  | Constructing objects step by step, fluent APIs, optional values, and validation before `Build()`.  | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/CreationalPatterns.Builder)       |
| **Prototype**                | Creating objects through cloning and understanding the difference between shallow and deep copies. | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Creational.Patterns.Prototype)    |

**Think about:** When does a constructor stop being enough? Who should decide which implementation to instantiate? When is copying an existing object more appropriate than rebuilding one?

### 🧩 Structural Patterns — *How should objects fit together?*

Structural patterns help combine components while keeping their responsibilities and dependencies manageable.

| Pattern       | What you'll learn                                                                                   | Branch                                                                                                   |
| ------------- | --------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------- |
| **Adapter**   | Integrating incompatible interfaces and keeping third-party or legacy APIs out of application code. | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Structural.Patterns.Adapter)  |
| **Decorator** | Adding logging, authorization, caching, or other behavior by wrapping an existing implementation.   | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/StructuralPatterns.Decorator) |

**Think about:** Do two components speak different interfaces? Do you need to extend a service without changing its core implementation?

### 🔄 Behavioral Patterns — *How should objects communicate and behave?*

Behavioral patterns focus on how work is delegated, how objects coordinate, and how behavior changes over time.

| Pattern                     | What you'll learn                                                                                      | Branch                                                                                                                |
| --------------------------- | ------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------- |
| **Strategy**                | Encapsulating interchangeable algorithms and choosing behavior without scattering conditional logic.   | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/BehavioralPatterns.Strategy)               |
| **Chain of Responsibility** | Passing requests through handlers, ordering processing steps, and short-circuiting a pipeline.         | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Behavioral.Patterns.ChainofResponsibility) |
| **Command**                 | Representing operations as objects so they can be invoked, queued, logged, or scheduled independently. | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Behavioral.Patterns.Command)               |
| **State**                   | Changing an object's behavior as its state changes and modeling valid transitions.                     | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Behavioral.Patterns.State)                 |
| **Observer**                | Implementing one-to-many notifications using subscriptions, C# events, and delegates.                  | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Behavioral.Patterns.Observer)              |
| **Mediator**                | Coordinating collaborating components through a central mediator instead of direct dependencies.       | [Explore →](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/tree/Behavioral.Pattern.Mediatr)                |

**Think about:** Should a request be handled by one component or move through a chain? Should a change notify subscribers? Should components communicate directly, or through a coordinator?

---

## 🔍 What You'll Find in a Pattern Branch

The format varies by pattern, but the focus remains consistent:

1. **The problem** — Start with an ordinary implementation and identify its limitations.
2. **The core idea** — Understand the participants, responsibilities, and relationships.
3. **C# examples** — Follow the pattern in code instead of relying only on diagrams.
4. **.NET connections** — Recognize similar ideas in common framework APIs and application architectures.
5. **Trade-offs** — Learn where the pattern is helpful and where simpler code may be better.
6. **Comparisons** — Distinguish related patterns that can look similar in practice.

Some branches also include variations, architectural discussion, or interview-style questions. The examples are written for learning; review and adapt them before using them in production systems.

## ⚙️ Where These Ideas Appear in .NET

Design patterns are not limited to textbooks. Once you understand their intent, you'll recognize related techniques in the .NET ecosystem:

| Design idea                 | Familiar .NET connection                                              |
| --------------------------- | --------------------------------------------------------------------- |
| **Builder**                 | `WebApplicationBuilder`, `ConfigurationBuilder`, `HostBuilder`        |
| **Chain of Responsibility** | ASP.NET Core middleware and exception-handler chains                  |
| **Decorator**               | Service wrappers and request pipeline behaviors                       |
| **Observer**                | C# `event`, delegates, and notifications                              |
| **Strategy**                | Interchangeable services and algorithms selected through abstractions |
| **Adapter**                 | Wrappers around legacy code, external APIs, and SDKs                  |
| **Command**                 | Command handlers, background work, and CQRS-style requests            |
| **State**                   | Workflows and state-dependent behavior                                |
| **Mediator**                | Component coordination and mediator-style request dispatching         |

These are **conceptual connections**, not a claim that every framework API is an exact Gang of Four implementation.

---

## 🚀 Getting Started

You can read any branch directly on GitHub, or explore the code locally:

```bash
git clone https://github.com/alicanyilmazz/ThinkingInDesignPatterns.git
cd ThinkingInDesignPatterns

# List the available remote branches
git branch -r

# Switch to an example branch
git switch CreationalPatterns.Builder
```

To switch to another pattern, run `git switch <branch-name>` (commit or stash any local changes first).

**Suggested learning route:** Begin with **Builder** or **Strategy**, continue with **Adapter** and **Decorator**, then explore **Observer**, **Command**, **State**, **Mediator**, and **Chain of Responsibility**. Read **Factory / Factory Method** and **Prototype** whenever object creation becomes your main focus.

## 💡 A Note on Design Patterns

Design patterns are tools, not rules.

A pattern is valuable when it addresses a real design pressure: changing requirements, excessive coupling, complex object creation, or difficult-to-test behavior. A simple class or method is often a better solution when that pressure does not exist.

This repository is an evolving learning project. The branch list above highlights the documented implementations; check [all repository branches](https://github.com/alicanyilmazz/ThinkingInDesignPatterns/branches) for updates.

If you find an issue, have an alternative implementation, or want to discuss a trade-off, feel free to open an issue or pull request.

---

**Explore the patterns, question the trade-offs, and keep the design as simple as it can be.**
