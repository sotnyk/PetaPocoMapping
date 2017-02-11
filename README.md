# PetaPocoMapping
Test project with custom mappers for Postgresql types Jsonb, Enum. PetaPoco microORM is used.

Steps:

1. Create Database named "test". Following credentials is used by defaults: admin/adminpass. Change it in the App.config, if you want.
2. Run SQL script "DbCreateScript.sql" to create proper structure for test database.
3. Compile and run App.

To use ENUM and jsonb types in your programs, you can register mapper constructed by ConstructEnumJsonMapper() method for your model:
```csharp
  Mappers.Register(typeof(JsonEnumMap), ConstructEnumJsonMapper());
```

SQL ENUM should have equivalent in C# with same-named members to be mapped.

Jsonb fields is recognized by type string in C# model's field and "jsonb" suffix in this field's name.

EnumMapper class from article http://stevedunns.blogspot.com/2011/08/fast-way-of-converting-c-enums-to.html is used.
