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
- const int LetterSetSize = 26：字符集大小，在这里我们使用英文字母，在忽略大小写和数字后字符集大小为26
- Dictionary<SchemeType, int> schemeCount：记录每一种类型的scheme的个数，因其需要被子类访问，因此访问权限为protected

### 保护成员
- SchemeState state：SchemeState是一个枚举类型，用于指示该对象目前的状态
- string encodeKey：加密密钥
- string decodeKey：解密密钥
- ConcurrentQueue\<string> processLog：加密/解密过程信息队列，使用线程安全队列
- keyIsValid()：虚函数，用于检测key是否合法
- encodeKeyIsValid()；虚函数，用于检测当前加密密钥是否合法
- decodeKeyIsValid()：虚函数，用于检测当前解密密钥是否合法


### 公共成员
- SchemeType Type：可读、子类可写属性，指示了该对象的加密解密类型，SchemeType是一个枚举类型，其中有该项目的所有算法
- State：只读属性，用于访问state
- Plain：属性，存储明文
- Cipher：属性，存储密文
- EncodeKey：虚属性，用于访问encodeKey
- DecodeKey：虚属性，用于访问DecodeKey
- bool ShouldOutput：属性，指示是否输出加密/解密过程
- Encode()：抽象函数，进行加密
- Decode()：抽象函数，进行解密
- Break()：抽象函数，对密文进行破解
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

#### 构造函数与虚构函数
- 如果派生类是对称加密算法（即加密密钥与解密密钥相同），则其参数列表中只有一个key。派生类构造函数应该调用基类Scheme的构造函数来进行大部分字段、属性的初始化。对称加密算法中的构造函数要在调用基类Scheme构造函数之后，需要确保加密密钥与解密密钥相同（详情见Substitution类）。
- 派生类需要在构造函数中给Type属性进行赋值

每一个派生类必须实现Encode、Decode、Save、Load方法。实现自身的构造函数与析构函数。并且适当地重写部分属性的get、set。

#### keyIsValid()函数
- 除非算法对密钥没有任何要求，否则就应该重写该函数

#### EncodeKey与DecodeKey属性
- 对于对称加密算法，两个属性中的set应当总是同时对encodeKey与decodeKey进行赋值
- 不应当在set中进行key的合法性检验

#### Encode()与Decode()
- 在进行加密/解密之前，应当先检查参数是否为空，对于不为空的参数，将其赋给对应属性。最后再检查加密/解密密钥的合法性。
- 即使对称加密算法中加密密钥与解密密钥总是相同，但我们始终在Encode()中使用EncodeKey，在Decode()中使用DecodeKey

## Caesar类

### EncodeKey属性
set中需要判断数据是否可解析为整数，并通过取模使其在[0,26)

--- 

## FrequencyAnalyst类
FrequenAnalyst是一个静态类，其使用词频分析来实现对解密正确可能性的计算。具体实现暂时在此略去，其方法应设计为线程安全的。

### 公共成员
- Analyze()：接收一个字符串，并计算该字符串为英文的概率
- Parser()：接收一个无空格字符串，对其添加空格进行分词