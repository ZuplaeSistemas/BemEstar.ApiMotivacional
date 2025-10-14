# Padrões de Projetos em C# Conexão API com Banco de Dados

## Visão Geral

A camada Data é responsável pela **conexão, configuração e execução de comandos SQL** no banco de dados PostgreSQL.  
Ela foi desenvolvida com base em **boas práticas de arquitetura** e aplicação de **padrões de projeto**, garantindo **reutilização**, **segurança** e **organização** do código.

A camada está dividida em três classes principais:

- **DatabaseConfig** → armazena a string de conexão.
- **DatabaseConnection** → gerencia a conexão com o banco (abrir e fechar).
- **SqlQuery** → contém métodos para gerar comandos SQL de forma genérica.

Ela é utilizada pela camada de **serviços**, que contém a lógica de negócio e usa a **BaseService** para padronizar as operações CRUD.

---

## Padrões de Projeto Utilizados

| Padrão                      | Função                                                   |
| --------------------------- | -------------------------------------------------------- |
| **Repository Pattern**      | Separa a lógica de acesso a dados da lógica de negócio.  |
| **Service Layer Pattern**   | Cria uma camada intermediária entre o banco e o domínio. |
| **Template Method Pattern** | Define o esqueleto dos métodos CRUD em `BaseService<T>`. |
| **Dependency Injection**    | Passa o `DatabaseConfig` para evitar dependência direta. |
| **Disposable Pattern**      | Garante fechamento seguro das conexões com `using`.      |

---

## Estrutura de Pastas

```
BemEstar.ApiMotivacional/
│
├── Data/
│   ├── DatabaseConfig.cs
│   ├── DatabaseConnection.cs
│   └── SqlQuery.cs
│
├── Service/
│   ├── BaseService.cs
│   └── MotivacionalService.cs
│
└── Models/
    └── Motivacional.cs
```

---

## DatabaseConfig.cs

Armazena a string de conexão usada pelo `DatabaseConnection`.

```csharp
namespace BemEstar.ApiMotivacional.Data
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; }

        public DatabaseConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
```

---

## DatabaseConnection.cs

Gerencia a conexão com o banco PostgreSQL usando o padrão **IDisposable**.  
Isso garante que o banco será fechado corretamente após o uso.

```csharp
using Npgsql;
using System;

namespace BemEstar.ApiMotivacional.Data
{
    public class DatabaseConnection : IDisposable
    {
        private readonly NpgsqlConnection _connection;

        public DatabaseConnection(DatabaseConfig config)
        {
            _connection = new NpgsqlConnection(config.ConnectionString);
        }

        public NpgsqlConnection GetConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            return _connection;
        }

        public void Dispose()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }
    }
}
```

---

## SqlQuery.cs

Contém métodos para **gerar comandos SQL de forma dinâmica**.  
Isso ajuda a manter o código limpo e centralizado, evitando repetições.

```csharp
using System.Linq;

namespace BemEstar.ApiMotivacional.Data
{
    public static class SqlQuery
    {
        public static string SelectAll(string table) =>
            $"SELECT * FROM {table};";

        public static string SelectById(string table) =>
            $"SELECT * FROM {table} WHERE id = @id;";

        public static string Insert(string table, string[] columns)
        {
            var cols = string.Join(", ", columns);
            var values = string.Join(", ", columns.Select(c => "@" + c));
            return $"INSERT INTO {table} ({cols}) VALUES ({values});";
        }

        public static string Update(string table, string[] columns)
        {
            var sets = string.Join(", ", columns.Select(c => $"{c} = @{c}"));
            return $"UPDATE {table} SET {sets} WHERE id = @id;";
        }

        public static string Delete(string table) =>
            $"DELETE FROM {table} WHERE id = @id;";
    }
}
```

---

## BaseService.cs

Classe abstrata que define o comportamento padrão para os serviços.  
Aqui aplicamos o **Template Method Pattern** — cada serviço específico (ex: `MotivacionalService`) herda esta estrutura e personaliza o que for necessário.

```csharp
using BemEstar.ApiMotivacional.Data;
using Npgsql;
using System;
using System.Collections.Generic;

namespace BemEstar.ApiMotivacional.Service
{
    public abstract class BaseService<T>
    {
        protected readonly string TableName;
        private readonly DatabaseConfig _config;

        protected BaseService(string tableName, DatabaseConfig config)
        {
            TableName = tableName;
            _config = config;
        }

        protected NpgsqlDataReader ExecuteReader(string query, Dictionary<string, object>? parameters = null)
        {
            var connection = new DatabaseConnection(_config);
            var command = new NpgsqlCommand(query, connection.GetConnection());

            if (parameters != null)
            {
                foreach (var p in parameters)
                    command.Parameters.AddWithValue(p.Key, p.Value);
            }

            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        protected void ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using var connection = new DatabaseConnection(_config);
            using var command = new NpgsqlCommand(query, connection.GetConnection());

            if (parameters != null)
            {
                foreach (var p in parameters)
                    command.Parameters.AddWithValue(p.Key, p.Value);
            }

            command.ExecuteNonQuery();
        }

        public abstract List<T> Read();
        public abstract T ReadById(int id);
        public abstract void Create(T model);
        public abstract void Update(T model);
        public abstract void Delete(int id);
    }
}
```

---

## MotivacionalService.cs (exemplo de uso)

Classe que herda de `BaseService` e implementa os métodos CRUD específicos da entidade `Motivacional`.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using BemEstar.ApiMotivacional.Data;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class MotivacionalService : BaseService<Motivacional>
    {
        public MotivacionalService(DatabaseConfig config) : base("motivacional", config) { }

        public override List<Motivacional> Read()
        {
            string commandText = SqlQuery.SelectAll(TableName);
            using var dataReader = ExecuteReader(commandText);
            return MotivacionalList(dataReader);
        }

        public override void Create(Motivacional model)
        {
            string commandText = SqlQuery.Insert(TableName, new[] { "texto", "autor", "created_at" });
            var parameters = new Dictionary<string, object>
            {
                {"texto", model.Texto },
                {"autor", model.Autor },
                {"created_at", model.CreatedAt }
            };
            ExecuteNonQuery(commandText, parameters);
        }

        public override Motivacional ReadById(int id)
        {
            string commandText = SqlQuery.SelectById(TableName);
            var parameters = new Dictionary<string, object> { { "id", id } };
            using var dataReader = ExecuteReader(commandText, parameters);
            return MotivacionalList(dataReader).FirstOrDefault();
        }

        public override void Update(Motivacional model)
        {
            string commandText = SqlQuery.Update(TableName, new[] { "texto", "autor", "created_at" });
            var parameters = new Dictionary<string, object>
            {
                {"texto", model.Texto },
                {"autor", model.Autor },
                {"created_at", model.CreatedAt },
                {"id", model.Id }
            };
            ExecuteNonQuery(commandText, parameters);
        }

        public override void Delete(int id)
        {
            string commandText = SqlQuery.Delete(TableName);
            var parameters = new Dictionary<string, object> { { "id", id } };
            ExecuteNonQuery(commandText, parameters);
        }

        // Método auxiliar para montar a lista de objetos
        private List<Motivacional> MotivacionalList(NpgsqlDataReader dataReader)
        {
            var list = new List<Motivacional>();
            while (dataReader.Read())
            {
                list.Add(new Motivacional
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Texto = dataReader["texto"].ToString(),
                    Autor = dataReader["autor"].ToString(),
                    CreatedAt = Convert.ToDateTime(dataReader["created_at"])
                });
            }
            return list;
        }
    }
}
```

---

## Benefícios dessa Arquitetura

- **Baixo acoplamento:** cada classe tem uma responsabilidade clara.
- **Reutilização:** a `BaseService` pode ser usada por qualquer entidade.
- **Escalabilidade:** fácil adicionar novas tabelas e serviços.
- **Organização:** separa camadas de dados, lógica e modelo.
- **Manutenibilidade:** facilita a atualização ou troca de tecnologias de banco.
