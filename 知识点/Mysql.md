* 使用命令行需要在mysql的安装目录下使用`mysql`命令
* MySql的安装：
    1. 下载mysql services压缩包
    2. 解压下载包，设置my.ini文件。此文件中的安装目录为解压目录，数据目录可以单独设置。
    3. 设置环境变量--在path节点下将mysql的bin目录写进去
    4. 初始化mysql `mysqld --initialize --console`此处输出临时密码需记住
    5. 安阳mysql服务  `mysqld --install`
    6. 启动mysql服务  `net start mysql`
    7. 输入用户名  `mysql -u root -p`
    8. 输入之前的临时密码
    9. 修改mysql 密码`alter user 'root'@'localhost' identified by 'xxxx';`
    10. 如果提示已安装了服务，可以使用 `mysqld --remove`移除之前的服务
[参考文章](https://www3.ntu.edu.sg/home/ehchua/programming/sql/MySQL_HowTo.html)
* mysql中使用utf8时，应该设置字符为utf8mb4
* MySQL默认情况下是否区分大小写，使用show Variables like '%table_names'查看lower_case_table_names的值，0代表区分，1代表不区分。

* Mysql的主键建议使用UUID，利于分库分表，且使用有序的UUID来提升性能(`UUID_TO_BIN`)
