using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Todo
{
    void Awake()
    {
        // todo 创建表加上工作表的名称
    }
    void Start()
    {
        // todo 基于工作表名称去寻找xlsx文件

        // todo 基于类型和数据路径去自动生成对应的 尝试获取数据函数

        // todo 输入表的名字，自动生成当前表的二进制配置

        // todo 自动生成所有Proto相关文件，增量形式--> 检查Excel的MD5 / 新增的Excel 对比本地的MD5 -->发生修改->重新生成对应的Proto文件->重打Dll->生成配置文件

    }
}
