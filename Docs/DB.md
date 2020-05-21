# DB
使用SQLite做持久化存储，通过ADO.NET框架中的Microsoft.Data.Sqlite库与SQLite交互。本程序使用的DB名为cipher_breaker.db

## 词频表
存储词频，属性有：
- 词名
- 频率

### 建表
```sql
create table word_frequency (
    word text not null primary key,
    frequency integer default 0 check(frequency > 0)
);
```