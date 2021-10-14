LinePutScript
===

<img src="Lineput.png" alt="Lineput" height="150px" />

LinePutScript是一种定义行读取结构和描述其内容的标准语言

可以应用于 保存 设置,资源,模板文件 等各种场景

类似于XML或Json但是比XML和Json更易于使用(理论上)

本类库可以更轻松的创建,修改,保存LinePutScript

提供开源源代码 可以自行修改支持更多功能

## 项目文件解释

### LinePutScript

一个LPS基本操作类,是所有LPS的根引用
如需操作lps文件,请使用这个文件

*'LinePutScript.Core'* 为.net Core版本

### LinePutScript.SQLHelper

一个操作数据库的帮助类 获得LPS结构的数据而非xml

*LinePutScript.SQLHelper.Core* 为.net Core版本

### LinePutScript.Lineput

LinePut是使用LinePutScript描述富文本的一种格式

这个类帮助转换LinePut为XAML FlowDocument

### LinePutScriptDataBase*

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
LinePutScript.LinePut
```
Install-Package LinePutScript.LinePut
```

2. 通过nuget.org

[LinePutScript](https://www.nuget.org/packages/LinePutScript/)

[LinePutScript.SQLHelper](https://www.nuget.org/packages/LinePutScript.SQLHelper/)

[LinePutScript.LinePut](https://www.nuget.org/packages/LinePutScript.LinePut)

