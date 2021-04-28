* 使用命令行需要在mysql的安装目录下使用`mysql`命令
* MySql的安装：
    1. 下载mysql services压缩包
    2. 解压下载包，设置my.ini文件。此文件中的安装目录为解压目录，数据目录可以单独设置。
    3. 设置环境变量--在path节点下将mysql的bin目录写进去
    4. 初始化mysql 
    5. 修改mysql 密码
* mysql中使用utf8时，应该设置字符为utf8mb4
* MySQL默认情况下是否区分大小写，使用show Variables like '%table_names'查看lower_case_table_names的值，0代表区分，1代表不区分。