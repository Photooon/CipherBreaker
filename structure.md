# CipherBreaker结构

下面是CipherBreaker的结构介绍

## Scheme类
Scheme类是一个抽象类，每一种算法都是Scheme类的派生类，继承方式为public，其下的派生类有：
- Caesar
- Vigenere
- Substitution
- Transposition
- Columnar

以上每一个类都应出现在SchemeType枚举类型中

### 静态成员
- Dictionary<SchemeType, int> schemeCount：记录每一种类型的scheme的个数，因其需要被子类访问，因此访问权限为protected

### 保护成员
- SchemeState state：SchemeState是一个枚举类型，用于指示该对象目前的状态
- string encodeKey：加密密钥
- string decodeKey：解密密钥
- ConcurrentQueue\<string\> processLog：加密/解密过程信息队列，使用线程安全队列


### 公共成员
- SchemeType Type：可读、子类可写属性，指示了该对象的加密解密类型，SchemeType是一个枚举类型，其中有该项目的所有算法
- State：只读属性，用于访问state
- Plain：属性，存储明文
- Cipher：属性，存储密文
- EncodeKey：属性，用于访问encodeKey
- DecodeKey：属性，用于访问DecodeKey
- bool ShouldOutput：字段，指示是否输出加密/解密过程
- Encode()：抽象函数，进行加密
- Decode()：抽象函数，进行解密
- Pause()：暂停加密/解密
- End()：终止加密/解密
- Save()：抽象函数，存储信息到文件，暂定
- Load()：抽象函数，从文件读取信息，暂定
- SchemeCount()：根据给定的SchemeType返回实例个数，若无参数则返回总实例个数
- ToString()：抽象函数，返回一个包含scheme信息的字符串

#### SchemeState
该枚举类型用于指示一个算法类对象的状态，有以下几种：
- Ready
- Running
- Pause
- Finish
- Over

其中Finish和Over的区别在于其加密/解密过程是否已经完成。

### Scheme的派生类
每一个派生类必须实现Encode、Decode、Save、Load方法。实现自身的构造函数与析构函数。并且适当地重写部分属性的get、set。

## Caesar类

### EncodeKey属性
set中需要判断数据是否可解析为整数，并通过取模使其在[0,26)

--- 

## FrequencyAnalyst类
FrequenAnalyst是一个静态类，其使用词频分析来实现对解密正确可能性的计算。具体实现暂时在此略去，其方法应设计为线程安全的。

### 公共成员
- Analyze()：接收一个字符串，并计算该字符串为英文的概率
- Parser()：接收一个无空格字符串，对其添加空格进行分词