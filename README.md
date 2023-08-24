LinePutScript
===

<img src="Lineput.png" alt="Lineput" height="150px" />

LinePutScript是一种数据交换格式定义行读取结构和描述其内容的标准语言

可以应用于 保存 设置,资源,模板文件 等各种场景

类似于XML或Json但是比XML和Json更易于使用(理论上)

本类库可以更轻松的创建,修改,保存LinePutScript

提供开源源代码 可以自行修改支持更多功能

## 项目文件解释

### LinePutScript

一个LPS基本操作类,是所有LPS的根引用
如需操作lps文件,请使用这个文件

*'LinePutScript.Core'* 为.net Core版本

## 如何使用:

### 安装

1. 通过Parckage Manager

LinePutScript
```
Install-Package LinePutScript
```

2. 通过nuget.org

   [LinePutScript](https://www.nuget.org/packages/LinePutScript/)

3. 下载nuget包

   [Nuget文件夹](https://github.com/LorisYounger/LinePutScript/tree/master/nuget)

## 关于LinePutScript语言

LinePutScript是一个最高4层*的数据储存语言
*最新1.4版本支持第四层,更多层次也可以使用套娃添加,但是较为繁琐

LineputScript由Line组成, Line由sub组成 sub为 `subname#subinfo` 

这是一个基础的LineputScript文件

```
line1name#line1info:|sub1name#sub1info:|sub2name#sub2info:|text
line2name#line2info:|money#100:|goods#item1,item2,item3:|
```

通过C# 读取信息如下

```C#
LpsDocument tmp = new LpsDocument("line1...item3:|");
tmp.First().ToString(); //line1name#line1info:|sub1name#sub1info:|sub2name#sub2info:|text
tmp.First().First().ToString();//line1name#line1info:|
tmp["line1name"]["sub1name"].Info; //sub1info
tmp[0].Text;//text
tmp[1][(gint)"money"];//100
tmp[1]["good"].GetInfos()[1];//item2
```

### 符号定义

#### \#

`#`分隔名称`Name`和信息`Info`,  
~~或者说分割键与值.~~

#### :|

`:|`分隔 `Sub` 与 `Sub` 或 `Line` 与 `Sub`

#### :\n|

lps文件支持多行并插入换行, 例如

```
详细文档:| 这是一份文件:
| 文件内容为空
```

```
详细文档:| 这是一份文件\n 文件内容为空
```

上方两者表达效果相同

#### :\n:

lps文件支持多行, 例如

```
详细文档:| 这是一份文件:
: 文件内容为空
```

```
详细文档:| 这是一份文件 文件内容为空
```

上方两者表达效果相同

### 使用方法

#### 案例:储存游戏设置

##### 读取LPS文件

```c#
//读取LPS文件
LpsDocument Save = new LpsDocument(File.ReadAllText("GAMEPATH\\save1.lps"));
//或创建新LPS文件
Save = new LpsDocument();
```

##### 获取和修改数据

案例要求:

* 储存金钱值为10000
* 添加类型电脑并储存电脑名字为 "我的电脑"
* 读取金钱值并加上500

*方法1 (lps1.0) -- 早期版本lps 繁琐操作*

```C#
Save.AddLine(new Line("money","10000"));//添加行 money 10000
Save.AddLine(new Line("computer",""));//添加行 compuer
Save.FindLine("computer").Add(new Sub("name","我的电脑")); //在computer行下面添加子类name和信息我的电脑

int Money = Convert.ToInt32((Save.FindLine("money").info)); //获得money储存的值
Save.FindLine("money").info = (Money + 500).ToString();//储存 money+500
```

*方法2 (lps1.1)  -- 上上版本lps 半繁琐操作*

```c#
Save.AddLine("money").InfoToInt = 10000; //添加行 money 10000
Save.FindorAddLine("computer").FindorAddLine("name").Info = "我的电脑";//查找行computer, 如果没找到,则创建一个新的. 在该computer行下查找或创建子类name,并修改其信息为 我的电脑

Save.FindorAddLine("money").InfoToInt += 500;//给money+500
```

*方法3 (lps1.2)  -- 上版本lps 半繁琐操作*

```c#
Save.AddLine("money").InfoToInt = 10000;
Save.FindorAddLine("computer").FindorAddLine("name").Info = "我的电脑";

Save["money"].InfoToInt += 500;//给money+500
```

*方法4 (lps1.2+)  -- 上版本lps后期 普通操作*

   ```c#
   Save.SetInt("money",10000);//设置 money 行 值(int)为10000
   
   Save["computer"].SetString("name","我的电脑");
   // 或这样 (对于string来说更方便)
   Save["computer"]["name"].Info = "我的电脑";
   
   Save.SetInt("money",Save.GetInt("money")+500);//给money+500
   ```

*方法5 (lps1.3) -- 最新版本lps 高级操作*

```c#
Save[(gint)"money"] = 10000; //设置 money 行 值(int)为10000
Save["computer"][(gstr)"name"] = "我的电脑";

Save[(gint)"money"] += 500;
```

*方法6 (lps1.8) -- 最新版本lps 无需操作*

```C#
//储存数据的类
public class SaveData{
    [Line]
    public int money = 10000;//设置 money 值(int)为10000
    public class Computer{
        [Line]
        public string name = "电脑默认名字";
    }
    [Line]
    public Computer computer = new Computer(){name = "我的电脑"};
}
//读取游戏数据
SaveData data = LPSConvert.DeserializeObject<SaveData>(Save);
data.money += 500;
//储存游戏数据
Save = LPSConvert.SerializeObject(data);
```

##### 储存LPS文件

```c#
//写入LPS源文件
File.WriteAllText("GAMEPATH\\save1.lps",Save.ToString());
```

##### 储存的LPS文件样式如下

```lps
money#10500:|
computer:|name#我的电脑:|
```

### 其他

更多用法及参数参见对象管理器
