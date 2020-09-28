# SQLite-Blob-POC

## SQLite 연결 함수
```csharp
Func<SqliteConnection> conn = () => new SqliteConnection($"Data Source=Application.db;Cache=Shared");
```
- `Microsoft.Data.Sqlite`는 연결 풀링을 구현하지 않음 (https://github.com/dotnet/efcore/issues/13837)

## 테이블 생성
```csharp
using (var c = conn())
{
    c.Open();

    var tableCommand = "CREATE TABLE IF NOT "
                        + "EXISTS USERS "
                        + "("
                        + "USER_ID INTEGER PRIMARY KEY, "
                        + "PASSWORD TEXT, "
                        + "USER_NAME TEXT, "
                        + "USER_GROUP TEXT, "
                        + "BLOB BLOB "
                        + ")";

    SqliteCommand createTable = new SqliteCommand(tableCommand, c);

    createTable.ExecuteReader();
}
```
![image](images/a42ee146b978bc7975394051c1e4756e.png)

## 삽입
```csharp
void Insert(UserInformation user)
{
    using (var c = conn())
    {
        c.Open();
        c.Execute(@"INSERT INTO USERS (PASSWORD, USER_NAME, USER_GROUP, BLOB)" +
                          "VALUES (:PASSWORD, :USER_NAME, :USER_GROUP, :BLOB)", user);
    }
}

Insert(new UserInformation
{
    BLOB = new byte[] {0x00, 0x00, 0x00, 0x01},
    USER_NAME = "Hong",
    PASSWORD = "GilDong",
    USER_GROUP = "Hero"
});
```

![image](images/154888fc4bdd5b4f120f0a1103cbad9b.png)

## 가져오기
```csharp
IEnumerable<UserInformation> FindAll()
{
    using (var c = conn())
    {
        c.Open();
        return c.Query<UserInformation>("SELECT * FROM USERS");
    }
}

foreach (var user in users.FindAll())
{
    Console.WriteLine(user.ToString());
}
```
![image](images/3d48760cd5241c2eaec6a53f7a8fbaf2.png)