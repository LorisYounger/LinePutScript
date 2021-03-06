LinePutScript
===

<img src="Lineput.png" alt="Lineput" height="100px" />

LinePutScript是一种定义行读取结构和描述其内容的标准语言

可以应用于 保存 设置,资源,模板文件 等各种场景

类似于XML但是比XML更易于使用(理论上)

本类库可以更轻松的创建,修改,保存LinePutScript

提供开源源代码 可以自行修改支持更多功能

详细使用方法见[LinePutScript\bin\Release\LinePutScript.xml](https://github.com/LorisYounger/LinePutScript/blob/master/LinePutScript/bin/Release/LinePutScript.xml)

## 项目文件解释

### LinePutScript

一个LPS基本操作类,是所有LPS的根引用
如需操作lps文件,请使用这个文件

'LinePutScript.Core' 为.net Core版本
'LinePutScript.Standard' 为.net Standard版本

### LinePutScript.SQLHelper

一个操作数据库的帮助类 获得LPS结构的数据而非xml

LinePutScript.SQLHelper.Core 为.net Core版本

### LinePutScriptDataBase

一个数据库构造类 通过映射LPS类型内容到内存, 从而实现基于LPS的快速数据库

### LPSDBHost*

一个简单的内存数据库通过使用LinePutScript.DataBase类实现
**\*注: 有内存无法正确回收的bug,可能需要重启解决**


## 如何使用:

1. 通过Parckage Manager

LinePutScript
```
Install-Package LinePutScript
```
LinePutScript.SQLHelper
```
Install-Package LinePutScript.SQLHelper
```

2. 通过nuget.org

[LinePutScript](https://www.nuget.org/packages/LinePutScript/)

[LinePutScript.SQLHelper](https://www.nuget.org/packages/LinePutScript.SQLHelper/)

3. 直接下载dll引用

[Releases](https://github.com/LorisYounger/LinePutScript/releases)

