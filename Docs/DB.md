# DB
使用SQLite做持久化存储，通过ADO.NET框架中的Microsoft.Data.Sqlite库与SQLite交互。本程序使用的DB名为cipher_breaker.db

## 词频表
存储词频，属性有：
- word：词名
- frequency：频率

### 建表
```sql
create table word_frequency (
    word text primary key,
    frequency integer default 0 check(frequency > 0)
);
```

## 操作记录表
存储用户的操作记录（加密、解密、破解），记忆化查询：
- id：自增id
- type：操作类型（加密、解密、破解）
- origin_text：初始文本
- key：密钥（操作类型为破解时，密钥为空）
- result_text：结果文本
- time：操作时间

### 建表
```sql
create table word_frequency (
    id integer primary key autoincrement,
    type integer not null,
    origin_text text not null,
    key text,
    result_text text,
    time date not null
)
```