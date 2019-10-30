【1】FreeSWITCH学习笔记

1、Windows安装包下载地址：

http://files.freeswitch.org/windows/installer/

2、源码下载地址：

http://files.freeswitch.org/freeswitch-1.2.10.tar.gz

3、FreeSWITCH后台模式启动

命令：freeswitch -nc（即No Console）

后台模式没有控制台

4、windows平台默认密码已修改为：liu1234

5、windows平台客户端分别为：X-Lite 和 EyeBeam

6、freeswitch默认使用UDP的5060端口。如果先启动客户端，会占用这个端口，所以，建议先启动freeswitch，再启动客户端。

7、如果运行freeswitch出现以下错误，是因为端口被占用了。

Cannot lock pid file /usr/local/freeswitch/run/freeswitch.pid.

8、如果输入fs_cli，提示如下类似错误：

[ERROR] fs_cli.c:1610 main() Error Connecting [Socket Connection Error]

说明FreeSWITCH没有启动 或 mod_event_socket没有正确加载，请检查TCP的8021端口是否处于监听状态或被其他进程占用。

9、freeswitch -nonat

freeSWITCH启动时默认会启用uPnP(或NAT_PMP)协议试图查找你的路由器是否支持并在你的路由器上“打洞”，如果你的路由器不支持该协议，这一步可能耗时比较长，因而影响启动速度。

所以，如果你只是在内网测试并且一天启动很多次的话，建议关掉这个选项。

组合参数（后台模式启动），启动命令：freeswitch -nc -nonat

10、修改配置XML文件后，必须先使用命令使之生效：reloadxml

11、客户端注册错误：

（1）提示408：Request Timeout

一般都是由于防火墙引起的。关闭防火墙（本地CentOS系统）：

临时关闭：service iptables stop

永久关闭：chkconfig iptables off

（2）提示408：forbidden

一般都是由于账号密码配置错误，鉴权没有通过而被禁止注册。

12、可以在freeswitch中使用originate命令发起一次呼叫。假设1000已经注册，那么命令：

originate user/1000 &echo

originate是freeswitch内部的一个命令，它用于控制freeswitch发起一个呼叫。

13、打印变量值。命令global_getvar 与 eval

global_getvar sound_prefix

eval ${sound_prefix}

global_getvar local_ip_v4

eval ${local_ip_v4}

14、自动加载模块的配置文件位置：

/usr/local/freeswitch/conf/autoload_configs/modules.conf.xml文件

15、用户目录默认配置文件在conf/directory/下，系统自带的配置文件为default.xml.

dial-string 一个至关重要的参数。

16、CentOS 系统让所有用户显示行号

输入命令：vim /etc/vimrc

在vimrc文件的最后添加：set nu

保存：wq

手动加载配置：source /etc/bashrc

这样不管是哪个用户在vim下都显示行号

17、FreeSWITCH启动后，在执行命令reloadxml时，错误提示：

+OK [[error near line 6290]: unexpected closing tag </condition>]

表示最近编辑过的xml文件格式有问题，标签形式不完整缺个“/”

18、reloadxml命令可以直接用快捷键F6执行。

console loglevel 7命令可以直接用快捷键F8执行

19、FreeSWITCH命令：查询已注册用户

sofia status profile internal  (REGISTRATIONS值：显示多少用户已注册）

具体谁是否注册？可以使用如下命令：

sofia status profile internal reg XXXX

20、默认快捷键集
  <cli-keybindings>
    <key name="1" value="help"/>
    <key name="2" value="status"/>
    <key name="3" value="show channels"/>
    <key name="4" value="show calls"/>
    <key name="5" value="sofia status"/>
    <key name="6" value="reloadxml"/>
    <key name="7" value="console loglevel 0"/>
    <key name="8" value="console loglevel 7"/>
    <key name="9" value="sofia status profile internal"/>
    <key name="10" value="sofia profile internal siptrace on"/>
    <key name="11" value="sofia profile internal siptrace off"/>
    <key name="12" value="version"/>
  </cli-keybindings>

21、dialplan中的测试条件可以迭加，但是不可以嵌套。

eg: 如下无效条件

<extension name="***">
 <condition field="***" expression="***">
 <condition field="***" expression="***">
  <action application="***"/>
 </condition>
 </condition>
</extension>

正确：
<extension name="***">
 <condition field="***" expression="***"/>
 <condition field="***" expression="***">
  <action application="***"/>
 </condition>
</extension>

22、xml dialplan支持非常丰富的功能。

但在测试或编写程序时，经常用到一些临时的或很简单的dialplan，如果每次都需要修改xml，不仅麻烦，而且执行效率低。

所以，一种短小、轻便的dialplan便因此而生了。

23、App的参数中可能会有逗号，因而会与默认的App间的逗号分隔符相冲突，以下的m语法形式将默认的逗号改为^分隔。

originate user/1000 ‘m:^playback/tmp/beep.wav^bridge:{ignore_early_media=true,originate_caller_id_number=1000}user/1001’inline

24、FreeSWITCH命令：show dialplan 命令？？？？？系统默认支持的Dialplan

25、一般来说，运营商都把应答时间作为计费的开始时间。

26、bridge操作是阻塞的，它会一直等到b-leg释放后才继续往下走。

27、常用的Dialplan App: play_and_get_digits与read类似，但它比read更高级。

28、在Dialplan中使用API命令。

<action application="set" data="api_result=${expr(1+1)}"/>

expr类似于UNIX中的expr命令，它计算一个表达式并输出结果，如：

freeswitch> expr 1+1

29、set 与 export的区别：

set程序仅仅会作用于当前的Channel(a-leg)

export程序可以将变量设置到两个Channel（a-leg和b-leg）上， 如果当时b-leg还没有创建，则会在创建时进行设置。

另外，export也可以通过nolocal参数将变量值限制仅仅设置到b-leg上：

<action application="export" data="nolocal:my_var=my_value">

更重要的是set也具有能往b-leg上赋值的能力：

<action application="set" data="export_vars=var1, var2, var3"/>

30、取消某些通道变量的定义。对它赋一个特殊的值（_undef_）或使用unset App，代码如下：
<action application="set" data="var1=_undef_"/>
<action application="unset" data="var1"/>

31、在实际应用中，可以截取变量值的部分操作。使用格式：“${var:位置:长度}”。

32、execute_extension与transfer类似，都需要Dialplan的“三要素”作为参数，但不同的是：

前者是临时执行一些Dialplan制定的App，它不会重新进入ROUTING阶段。

33、获取SIP消息的方法：

在freeswitch命令上上执行：sofia global siptrace on

如果想关闭trace，可执行命令：sofia global siptrace off

34、FreeSWITCH相关协议

SIP：会话发起协议

SDP：会话描述协议

RTP：实时传输协议 

RTCP:实时传输控制协议

35、SIPProfile相当于一个SIPUA，通过各种不同的配置参数可以配置一个UA的行为。一个系统中可以有多个SIPProfile，

每个SIPProfile都可以监听不同的IP地址和端口对。

36、sofia的配置文件是conf/autoload_configs/sofia.conf.xml.

37、freeswitch命令行：sofia status profile internal 查看profile的参数配置结果

38、CentOS6.5查看tomcat版本信息：

命令1：cd /usr/local/tomcat/bin

命令2：./catalina.sh version

39、启动tomcat命令：

/usr/local/tomcat/bin ./startup.sh

40、web端访问不到虚拟机的tomcat服务器：

关闭防火墙命令 ：service iptables stop

41、FS常用API和命令：

（1）命令：... 退出FS 或者 fs_cli

（2）命令：shutdown 退出FS 和 fs_cli

（3）命令：sofia status profile internal reg 查询分机注册信息

（4）命令：sofia profile internal siptrace on/off 显示SIP调试信息

（5）命令：/event all # 打开所有事件的调试开关

（6）命令：/event MESSAGE 打开即时消息的调试开关

（7）命令：sofia global debug presence 状态呈现调试信息

（8）命令：sofia status SIP 协议栈状态

42、FS 常用APP

（1）set/export set用来设置通道变量

（2）answer 发送200 OK

（3）bridge

（4）sleep

（5）transfer

（6）bind_meta_app

（7）bind_digit_action

（8）conference

43、FS_CLI

FS默认是以控制台方式运行：

启动：./freeswitch

关闭：fsctl shutdown

停止：freeswitch -stop

若想要在后台运行，在命令行提供参数-nc，即命令：

freeswitch -nc

当FS在后台运行时，可通过运行fs_cli启动一个控制台并连接到FS。

FS_CLI命令行启动：fs_cli -H 127.0.0.1 -P 8021 -p ClueCon -d 7

44、配置从其它服务器通过fs_cli访问FreeSWITCH(????)

45、更改主叫号码。主叫名称和主叫号码

命令1：originate user/1000 &echo XML default ‘Seven Du’ 7777

命令2：originate user/1000 &echo XML default 'Liu yong' 6666

46、originate 命令最后一个参数超时秒数，是指对方收到我们INVITE消息后 ，不回复100 Trying消息的时间。

47、防止命令阻塞bgapi。originate命令是阻塞的，因此如果执行上述命令，则无法输入其他命令或取消该呼叫。

解决这个问题有三种办法：

（1）使用bgapi 如：bgapi originate user/1000 &echo XML default 'Liu yong' 7777

向freeswitch发送bgapi命令，后台执行，非阻塞执行。

再具体一点说，即使用bgapi的命令执行以后，敲回车键，控制台还可以再执行其他命令。

不使用bgapi的命令执行以后，敲回车键，控制台不再可以输入其他命令。

（2）若不使用bgapi 如：originate user/1000 &echo XML default 'Liu yong' 7777

该命令执行以后，再敲回车键，控制台不可再输入其他命令，即阻塞执行。

这时想要输入其他命令或取消该呼叫，必须开启另外一个fs_cli客户端。有两种处理方法：

2.1 使用show channels 找到该呼叫的UUID，然后执行uuid_kill

2.2 直接执行hupall挂断所有通话

48、使用通道变量改变呼叫字符串。

命令：originate {origination_caller_id_name='Liu yong',origination_caller_id_number=6666}user/1000 &echo

特别备注：大括号与user之间没有空格（反正我踩雷了！特此提醒）

也支持每个通道变量分别用大括号包起来，如下等价：

命令：originate {origination_caller_id_name='Liu yong'}{origination_caller_id_number=6666}user/1000 &echo

49、主叫号码

命令1：originate {origination_caller_id_number=6666}user/1000 &bridge(user/1001)

由于freeswitch发现a-leg是一个回呼，当a-leg接听后，再作为主叫去呼叫b-leg时，会进行主叫号码翻转。

命令2：originate {origination_caller_id_number=6666}user/1000 &bridge({origination_caller_id_number=8888}user/1001)

改变1000这个主叫号码为8888

命令3：originate {effective_caller_id_number=6666}user/1000 &bridge(user/1001)

effective_caller_id_number变量设置在a-leg上，但影响b-leg的主叫号码显示（来电显示）

命令4：originate {origination_caller_id_number=7777}user/1000 &bridge({origination_caller_id_number=8888}user/1001)

命令5：originate {origination_caller_id_number=7777}{effective_caller_id_number=8888}user/1000 &bridge(user/1001)

命令4与命令5等价，请结合前三个命令自行分析。

50、API: sofia_contact 作用查找数据库，找到实际注册1001的Contact地址，并返回真正的呼叫字符串。

51、当FreeSWITCH呼叫的号码为错误字符串时，提示如下：

CHAN_NOT_IMPLEMENTED:表示这种Channel的类型没有实现。

52、

同振：originate命令可以同时呼叫两个用户，两个话机都会振铃，哪个先接听则接通哪个，另一路会自动挂断。

命令：originate user/1000,user/1001 &echo

顺振：第一个呼叫失败，则呼叫第二个。

命令：originate user/1000|user/1001 &echo

53、三个等价命令：

（1）originate user/1000 9196 :: 在1000接听后进入Dialplan，找到9196这个exten，然后再执行echo

（2）originate user/1000 &echo :: 向外发起一个呼叫，建立一个Channel，对方接听后在本端执行一个App，此App即为echo

（3）originate user/1000 inline :: 内联Dialplan

54、更改主叫号码的命令：

originate user/1000 &echo XML default ‘Seven Du’ 777

注意：

（1）FreeSAWITCH发起呼叫默认使用的主叫号码是000000000

（2）参数中有空格，需要用单引号引起来。

55、编译安装mod_xtgssd4fs

（1）文件放置位置：/usr/src/freeswitch/src/mod/xml_int

（2）如果编译不过，照这个：

2.1：在文件configure.ac[位置：/usr/src/freeswitch]中

添加src/mod/xml_int/mod_xtgssd4fs/Makefile

2.2：依次分别执行如下四个命令：

   aclocal
   autoconf
   automake --add-missing
   ./configure操作

（3）完成以上后。再编译源文件，命令：make install

56、测试TTS的Dialplan：

<extension name="TTS">
 <condition field="destination_number" expression="^1234$">
  <action application="answer"/>
  <action application="speak" data="fliter|rms|hello, Welcome to FreeSWITCH"/>
 </condition>
</extension>

57、以“phrase:”开头的文件参数表示这里要播放一个Phrase Macro，冒号后面跟的是参数。

58、编译最新的master版本到默认位置：

git clone git://git.freeswitch.org/freeswitch.git freeswitch-master

cd freeswitch-master

./bootstrap.sh && ./configure && make && make install

59、编译1.2版本并安装到/usr/local/freeswitch-1.2

git clone git://git.freeswitch.org/freeswitch.git freeswitch-1.2

cd freeswitch-1.2

git checkout v1.2.stable

./bootstrap.sh && ./configure --prefix=/usr/local/freeswitch-1.2

make && make install

60、编译模块。编译mod_callcenter为例：

（1）到FreeSWITCH源代码目录下使用以下命令编译安装：

#make mod_callcenter-install

（2）再到FreeSWITCH控制台上加载该模块：

#load mod_callcenter

（3）如果需要FreeSWITCH启动时自动加载该模块，可以编辑

conf/autoload_configs/modules.conf.xml文件，去掉与该模块相关的行注释，即如下：

<load module="mod_callcenter"/>

61、坐席类型：

（1）onhook坐席。如果有电话分配到该坐席时，FreeSWITCH将会呼叫坐席。

（2）offhook坐席。坐席需要先拨入FreeSWITCH建立一个呼叫，当有来话时则可以立即与坐席桥接起来。

62、呼叫路由注意点：

（1）通过FreeSWITCH控制台拨号进行呼叫，不执行路由过程。

（2）通过客户端X-Lite或EyeBeam拨号，执行拨号路由过程。

63、呼叫中心模块

（1）mod_callcenter模块并不是默认就编译安装的。因而我们首先要编译安装它。

切换到FreeSWITCH源代码目录（/usr/src/freeswitch）下，使用以下命令编译安装：

# make mod_callcenter-install

（2）编译安装完成后，需要到控制台加载该模块：

freeswitch> load mod_callcenter

（3）如果需要FreeSWITCH启动时自动加载该模块，可以编辑

conf/autoload_configs/modules.conf.xml，去掉与该模块相关的行注释，即如下：

<load module="mod_callcenter"/>

（4）进行静态坐席的配置。mod_callcenter默认的配置文件是

conf/autoload_configs/callcenter.conf.xml，其中默认已经配置了一个support@default队列。

其中strategy参数指定了队列的分配方式，它的值longest-idle-agent说明要优先选择等待时间最长的坐席分配。

（5）接着，来配置一个坐席，坐席的名字是1005@default，配置如下：

  <agents>
    <!--<agent name="1000@default" type="callback" contact="[call_timeout=10]user/1000@default" status="Available" max-no-answer="3" wrap-up-time="10" reject-delay-time="10" busy-delay-time="60" />-->
    <agent name="1005@default" type="callback" contact="[call_time_out=10]user/1005"
           status="Avalable" max-no-answer="3" wrap-up-time="10"
           reject-delay-time="10" busy-delay-time="60"/>
  </agents>

（6）同理，再配置其他坐席，如下1006@default、1007@default等，如下：
  <agents>
    <!--<agent name="1000@default" type="callback" contact="[call_timeout=10]user/1000@default" status="Available" max-no-answer="3" wrap-up-time="10" reject-delay-time="10" busy-delay-time="60" />-->
    <agent name="1005@default" type="callback" contact="[call_time_out=10]user/1005"
           status="Avalable" max-no-answer="3" wrap-up-time="10"
           reject-delay-time="10" busy-delay-time="60"/>

    <agent name="1006@default" type="callback" contact="[call_time_out=10]user/1006"
           status="Avalable" max-no-answer="3" wrap-up-time="10"
           reject-delay-time="10" busy-delay-time="60"/>
    
     <agent name="1007@default" type="callback" contact="[call_time_out=10]user/1007"
           status="Avalable" max-no-answer="3" wrap-up-time="10"
           reject-delay-time="10" busy-delay-time="60"/>
  </agents>

（7）然后，还需要配置tier，以将坐席与队列关联起来。

如下面的配置将三个坐席与队列support@default关联起来。

  <tiers>
    <!-- If no level or position is provided, they will default to 1.  You should do this to keep db value on restart. -->
    <!-- <tier agent="1000@default" queue="support@default" level="1" position="1"/> -->
    <tier agent="1005@default" queue="support@default" level="1" position="1"/>
    <tier agent="1006@default" queue="support@default" level="1" position="1"/>
    <tier agent="1007@default" queue="support@default" level="1" position="1"/>
  </tiers>

（8）OK，以上完成之后，最后再需要配置呼叫路由。

即当有电话呼叫support时，可以使用如下Dialplan将电话转到该callcenter队列中，如下：

    <!--Cellcenter Example-->
    <extension name="Callcenter Example">
      <condition filed="destination_number" expression="^support$">
       <action application="answer"/>
       <action application="callcenter" data="support@default"/>
      </condition>
    </extension>

（9）以上设置均完成后，就重新加载模块使之生效。

freeswitch> reloadxml

freeswitch> reload mod_callcenter

（10）现在可以动态管理队列和坐席。

mod_callcenter提供了一个callcenter_config的API命令，用于管理与该模块相关的各种资源。

比如，可以使用如下命令手工将坐席签入：

freeswitch> callcenter_config agent set status 1005@default 'Available'

（11）将坐席的状态置为Logged Out，就不会再有电话分配到该坐席了。

freeswitch> callcenter_config agent set status 1005@default 'Logged Out'

（12）当想知道当前有所有的坐席时，可以使用如下命令：

freeswitch> callcenter_config agent list

（13）当然，如果坐席没有在配置文件中进行配置，可以使用命令添加，如下：

freeswitch> callcenter_config agent add 1007@default callback

注意，还需要使用下列命令给它的设置相关的参数，如下contact:

freeswitch> callcenter_config agent set contact 1007@default user/1007

（14）也可以使用以下命令列出当前队列：

freeswitch> callcenter_config queue list

（15）列出当前梯队：

freeswitch callcenter_config tier list

64、数据库。命令行退出sqlite3数据库两种方式：

（1）.quit

（2）ctrl + D

65、编解码与修改全局变量。修改配置文件中的全局变量，一般需要重启FreeSWITCH才能使之生效。

而实际应用中，不允许随便重启服务器。为了不重启FreeSWITCH服务器，有两种解决方法：

（1）修改完毕，给FreeSWITCH进程发送一个SIGHUP信号，让它重新解析全局变量：

# kill -HUP <FreeSWITCH的进程号>

但是，此命令只是保证FreeSWITCH正确解析了这些全局变量，而实际上，这些变量值是在sofia profile中引用的，

因而还需要在FreeSWITCH控制台执行以下命令重读sofia的配置，如下：

freeswitch> sofia profile internal rescan

freeswitch> sofia profile external rescan

当然，也可以不执行以上两个重读命令。一步到位，重新加载一次该模块：

freeswitch> reload mod_sofia

（2）可以不修改全局变量，毕竟使用它的地方是在两个profile的配置中。

可以把视频编码附加到变量引用的后面，具体如下：

<param name="inbound-codec-prefs" value=“$${global_codec_prefs}, H264, VP8”/>

<param name="outbound-codec-prefs" value=“$${global_codec_prefs}, H264, VP8”/>

或者

<param name="inbound-codec-prefs" value=“PCMA, PCMU, H264, VP8”/>

<param name="outbound-codec-prefs" value=“PCMA, PCMU, H264, VP8”/>

最后，不论选择方式（1）或方式（2）为了验证是否生效。可以如下命令：

freeswitch> sofia status profile internal

66、话单写入失败备份目录。

如果由于某种原因引起写入话单失败，服务器返回非“200 OK”的消息，

则FreeSWITCH就会将这张话单存入一个本地的XML文件中（默认位置在log/xml——cdr目录中），以免丢失话单。

67、新建一个FreeSWITCH实例，仅配置不同，实际操作步骤如下：

（1）复制一份新环境

mkdir /usr/local/freeswitch2;   // 新建freeswitch2目录

cp -R /usr/local/freeswitch/conf /usr/local/freeswitch2/; // 拷贝一份旧的配置文件

mkdir /usr/local/freeswitch2/log;  // 给新建的目录创建一个日志目录

mkdir /usr/local/freeswitch2/db;   // 给新建的目录创建一份数据库目录

ln -sf /usr/local/freeswitch/sounds /usr/local/freeswitch2/sounds  // 给新建的目录超链接一份旧的声音文件

（2）修改新配置中的一些配置参数以防止端口冲突。修改Event Socket的端口号。

文件位置：conf/autoload_configs/event_socket.conf.xml

修改内容：8021为9021

（3）修改全局变量conf/vars.xml，把其中的5060、5080也改成其他的值，如7060和7080。

68、CentOS系统修改网卡配置

（1）vi /etc/sysconfig/network-scripts/ifcfg-eth0 进入网卡配置

（2）重启网卡 service network restart

（3）设置虚拟机的网卡连接方式：桥接网卡

69、CentOS系统不支持rz、sz导入导出文件命令。

安装命令：yum install lrzsz

70、FreeSWITCH与FreeSWITCH互连，拨打电话命令：

bgapi originate user/1001 &bridge(sofia/external/sip:1006@10.10.17.23:5080)

71、IP地址鉴权方式。

在汇接局上关闭5080端口，而让所有来话都送到5060端口，5060端口上的来话是需要先鉴权才能路由的。

在这种汇接模式中，一般会使用IP地址鉴权的方式，而IP地址鉴权就会用到ACL（即访问控制列表）。

72、汇接。

<!--

D 作为汇接局，A、B、C分别为端局。

-->

<!--

A欲呼叫B上的用户B1000，A端的default.xml配置文件，其diaplan如下：

-->

<extension name="D">
    <condition field="destination_number" expression="^([B-Z]. *)$">
        <action application="bridge" data="sofia/external/sip:$1@10.10.17.26:5080"/>
    </condition>
</extension>

<!--
在D端上，收到5080端口的呼叫请求后，查找public dialplan对来话进行路由。其dialplan设置如下：
-->
<!--
当被叫号码首位是D时，表示是一个本地用户，那么“吃掉”首位的“D”，然后把路由转transfer到default dialplan进行处理
-->
<extension name="D">
    <condition field="destination_number" expression="^D(. *)$">
        <action application="transfer" data="$1 XML default"/>
    </condition>
</extension>

<!--
对于被叫号码不在本地的用户，使用下面的dialplan，其配置如下：
正则表达式匹配所有除D以外的A到Z开头的被叫号码。
以有人拨打B1000为例，匹配成功后，$1值为B，$2的值为1000，所以，bridge的参数即为：
sofia/external/sip:1000@10.10.17.B:5080，相当于在汇接局D上“吃掉”了被叫号码的最首位B。
-->
<extension>
    <condition field="destination_number" expression="^([A-CE-Z])(.*)$">
        <action application="set" data="bypass_media=true"/>
        <action application="bridge" data="sofia/external/sip:$2@10.10.17.$1:5080"/>
    </condition>
</extension>

73、添加网关。添加网关步骤：将下面内容存放到conf/sip-profiles/external/gw_a

（1）将网关设置内容配置存放到conf/sip-profiles/external/gw_a.xml文件中，

（2）在FreeSWITCH控制台使用命令sofia profile external rescan命令使之生效。

74、电话号码透传。

当端局A作为PBX时，一般来说，端局A不允许主叫号码透传，即不管是F上的哪个分机往外打电话，都会在对方的话机上显示1000这个主叫号码。

当然，我们也可以设置允许1000往外打电话时进行主叫号码透传。操作步骤：

在FreeSWITCH_A上找到1000这个用户的配置文件（1000.xml），将下面的行注释掉：

<variable name="effective_caller_id_number" value="1000"/>

其中，effective_caller_id_number就表示1000这个用户如果发起呼叫时对外显示的号码，默认的设置就是1000。

注释掉该项后，就会根据来电号码对外进行发送。

75、DID（Dial In Directly）：对内直接呼叫。

76、CentOS 更换源。CentOS 6.x 安装阿里源

命令：wget -O /etc/yum.repos.d/CentOS-Base.repo http://mirrors.aliyun.com/repo/Centos-6.repo

下载成功后执行命令：

（1）yum clean all

（2）yum makecache

77、CentOS源错误apt.sw.be。

错误提示：http://apt.sw.be/redhat/el6/en/x86_64/dag/repodata/repomd.xml: [Errno 14] PYCURL ERROR 6 - "Couldn't resolve host 'apt.sw.be'"

解决方案：/etc/yum.repos.d文件夹中，找到包含apt.sw.be的URL文件，然后将其改名。

比如本地CentOS6.5，发现为dag.repo文件。

解决命令（比如本地CentOS6.5）：mv dag.repo dag.repo.bak

78、找不到“hiredis.h”文件。从redis安装源代码中拷贝一份hiredis目录到/usr/local/include目录下。

79、xtaud配置错误：

[root@CentOSLy debug]# cmake -DBUILD_STATIC_LIBS=ON -DBUILD_SHARED_LIBS=OFF -DARCHIVE_INSTALL_DIR=. -DJSONCPP_WITH_CMAKE_PACKAGE=ON -DJSONCPP_WITH_TESTS=ON ../..

bash: cmake: command not found

安装cmake。使用命令：yum -y install cmake

安装cmake依赖gcc-c++ 需要先安装gcc-c++，默认的gcc版本不支持C++11，所以需要手动安装。

80、CentOS安装包查询。

查询路径：cd /etc/yum.repos.d

查询命令：yum search 软件名

eg: yum search cmake

如果源不支持，需要手动安装（1、源码 2、命令：./configure 3、make 4、make install）。

81、find查找命令。

命令格式：find / -name ×××查找文件目录位置

82、redis设置密码

[root@CentOSLy bin]# redis-cli

127.0.0.1:6379> config set requirepass ****

OK

127.0.0.1:6379> config get requirepass

(error) NOAUTH Authentication required.

127.0.0.1:6379> auth ****

OK

127.0.0.1:6379> config get requirepass

1) "requirepass"

2) "****"

127.0.0.1:6379>

83、ESL，Event_Socket Library，事件套接字库

84、SIP（会话初始协议）处于会话层，RTP是基于UDP的，处于传输层。

85、event。event用于订阅事件。让FreeSWITCH把相关的事件发送过来。

其中，type有三种plain（纯文本）、json、xml三种。默认是plain类型。

86、SIP协议不同于http协议的客户端服务端模式，而是对等的点对点通信模式。

（1）代理服务器

（2）重定向服务器

（3）注册服务器

（4）背靠背用户代理（B2BUA）

在SIP的世界中，所有UA都是平等的。这个特性也是sip协议与http协议区别。

87、边界会话控制器Session Border Controller, SBC

它主要位于一堆SIP服务器的边界，用于隐藏内部服务器的拓扑结构、抵御外来攻击等。SBC可能是一个代理服务器，也可能是一个B2BUA。

88、打开或关闭sip详细日志

sofia profile internal siptrace on

sofia profile internal siptrace off

89、在管理控制台上设置日志级别

console loglevel （0~7）数值越大级别越大

启动设置日志级别，修改vars.xml文件：

<X-PRE-PROCESS cmd="set" data="console_loglevel=4"/>

90、目前线上FS版本号V1.4.14

91、编译Gssd4FS参考：

（1）在文件configure.ac添加src/mod/xml_int/mod_xtgssd4fs/Makefile

autoreconf -ivf

（2）执行如下四个命令

   aclocal
   autoconf
   automake --add-missing
   ./configure

svn://svn.union400.com:3691/project/03_源码/05_通信平台/05_gssdFS版/v.1.1.0 Spc005

92、

在Linux平台上，能直接得到FreeSWITCH的进程号（需要root权限）：netstat -anp | grep 5060

看进程是否存在：

ps aux | grep freeswitch

93、抓包工具

（1）tcpdump -i eth0 port 5060 and ip src 10.10.17.37

（2）tcpdump -i eth0 port 5060 and host 10.10.17.37

94、VS2017编译FreeSWITCH遇到的问题集

（1）us_text.c文件

Error:C2001 常用中含有换行符问题

解决方法：修改为utf-8 BOM编码

（2）libx264

找不到windows sdk版本8.1，切换为本地windows sdk版本

（3）C2220 警告被视为错误，没有生成“object”文件

项目zlib

项目libpng

解决方法：点击项目，右击选择属性->配置属性->c/c++->常规，将“警告视为错误”的选项改为“否”。就可以

（4）error MSB6006: “cmd.exe”已退出 代码为 1

项目libV8

解决方法：移除这个项目

95、Windows系统安装FreeSWITCH

默认安装路径：C:\Program Files\FreeSWITCH

配置文件路径：C:\Program Files\FreeSWITCH\conf

编译输出目录：F:\Git_FreeSWITCH\freeswitch\Win32\Debug\FreeSwitchConsole.exe

96、linux CentOS系统安装
yum install -y autoconf automake libtool gcc- c++ ncurses- devel make zlib- devel libjpeg- devel yum install –y openssl- devel e2fsprogs- devel curl- devel pcre- devel speex- devel sqlite- devel

97、freeswitch 查询版本号

# freeswitch -verison

98、FreeSWITCH运行参数

（1）-nf 不允许Fork新进程

（2）-u [user] 启动后以非root用户user身份运行

（3）-g [group] 启动后以非root组group身份运行

（4）-version 显示版本信息

（5）-waste 允许浪费内存地址空间，FreeSWITCH仅需240KB的栈空间，你可以使用ulimit -s 240限制栈空间使用，或使用该选项忽略警告信息

（6）-core 出错时进行内核转储

（7）-rp 开启高优先级（实时）设置

（8）-lp 开启低优先级设置

（9）-np 普通优先级

（10）-vg 在valgrind下运行，调试内存泄漏时使用

（11）-nosql 不使用SQL，show channels类的命令将不能显示结果

（12）-heavy-timer 更精确的时钟，可能会更精确，但对系统要求更高

（13）-nonat 如果路由器支持uPnP或NAT-PMP，则FreeSWITCH可以自动解决NAT穿越问题。如果路由器不支持，则该选项可以使启动更快。

（14）-nocal 关闭时钟核准。FreeSWITCH理想的运行环境是1000Hz的内核时钟。如果你在内核时钟小于1000Hz或在虚拟机上，可以尝试关闭该选项

（15）-nort 关闭实时时钟

（16）-stop 关闭FreeSWITCH，它会在run目录中查找PID文件

（17）-nc 启动到后台模式，没有控制台

（18）-ncwait 后台模式，等待系统完全初始化完毕之后再退出父进程，隐含“-nc”选项

（19）-c 启动到控制台，默认Options to control location of files

（20）-base [confdir] 指定其他的基准目录，在配置文件中使用$${base}

（21）-conf [confdir] 指定其他的配置文件所在目录，需与-log、-db合用

（22）-log [logdir] 指定其他的日志目录

（23）-run [rundir] 指定其他存放PID文件的运行目录

（24）-db [dbdir] 指定其他数据库的目录

（25）-mod [moddir] 指定其他模块目录

（26）-htdocs [htdocsdir] 指定其他HTTP根目录

（27）-scripts [scriptsdir] 指定其他脚本目录

（28）-temp [directory] 指定其他临时文件目录

（29）-grammar [directory] 指定其他语法目录

（30）-certs [directory] 指定其他SSL证书路径

（31）-recordings [directory] 指定其他录音目录

（32）-storage [directory] 指定其他存储目录（语音信箱等）

（33）-sounds [directory] 指定其他声音文件目录

99、FreeSWITCH中修改注册用户的密码

（1）在FreeSWITCH系统中，所有用户的密码默认为1234，该设置在 \conf\vars.xml 中，如下所示：

<X-PRE-PROCESS cmd="set" data="default_password=1234"/>

若需要修改默认密码的话，直接修改该处即可。

（2）但用户的密码和默认密码怎么关联起来？

请看下面的配置文件，以用户1000为例，打开 \conf\directory\default\1000.xml，找到如下设置：

<param name="password" value="$${default_password}"/>，即可找到他们之间的联系。

若需要修改某用户的密码，直接修改 value 值即可。

（3）修改以上配置完成后，通过FS_CLI.exe运行reloadxml即可。

100、FreeSWITCH配置同振和顺振

通过FreeSwitch可以对多个终端进行呼叫，依据振铃顺序不同，可以分为：同振和顺振。

同振是指多个终端同时振铃；顺振是指多个终端顺序振铃。

下面是同振的配置：某用户拨叫2000这个接入号码，希望1000和1001同时振铃，其中任一个接听来话，另一个停止振铃。

（1）在 \conf\dialplan\default.xml中添加如下内容：
<extension name="group_dial_sim">                            
    <condition field="destination_number" expression="^2000$">                                
        <action application="bridge" data="sofia/internal/1000@127.0.0.1,sofia/sip/1001@127.0.0.1"/>                            
    </condition>                       
</extension>             

在FS_CLI.exe中运行reloadxml即可。 

下面是顺振的配置，某用户拨叫2000这个接入号码，希望1000和1001顺序振铃，其中任一个接听来话，另一个停止振铃。

（2）在 \conf\dialplan\default.xml中添加如下内容：
<extension name="group_dial_seq">
    <condition field="destination_number" expression="^2000$">                                
        <action application="bridge" data="sofia/internal/1000@127.0.0.1|sofia/sip/1001@127.0.0.1"/>                            
    </condition>                     
</extension> 

在 FS_CLI.exe中运行reloadxml即可。


这次来说说 freeSWITCH 的安装和配置。

1) 安装

freeSWITCH 下载页面：https://freeswitch.org/confluence/display/FREESWITCH/Installation 。

我们 Windows 7 下，使用 1.6.17 x64 版本，下载地址为：http://files.freeswitch.org/windows/installer/x64/FreeSWITCH-1.6.17-x64-Release.msi。

选择完整安装，一路 Next 即可。

安装完毕后，需要做一些配置。

2) wss 配置

因为 WebRTC 需要 https ，对应的 WebSocket 也要 SSL 。freeSWITCH 支持 SSL 但默认没打开。

wss 配置分两部分，
conf/vars.xml 有两个开关，打开。类似下面：

<X-PRE-PROCESS cmd="set" data="internal_ssl_enable=true"/> 

<X-PRE-PROCESS cmd="set" data="external_ssl_enable=true"/>

    1
    2
    3

conf/sip_profiles/internal.xml 中确保下面两个配置打开：

<!-- for sip over websocket support -->
<param name="ws-binding"  value=":5066"/>

<!-- for sip over secure websocket support -->
<!-- You need wss.pem in $${certs_dir} for wss or one will be created for you -->
<param name="wss-binding" value=":7443"/>    

    1
    2
    3
    4
    5
    6

SIP 服务的端口是 5060 ，WebSocket（ws）服务的端口是 5066 ， wss 端口是 7443 。

3）局域网支持

我在局域网内进行测试，得做一个 ACL 配置，否则调不通。

conf/autoload_configs/acl.conf.xml 中，加入下面配置：

<list name="localnet.auto" default="allow">
</list>

    1
    2

然后，conf/sip_profiles/internal.xml 中加入下列配置：

<param name="apply-candidate-acl" value="localnet.auto"/>

    1

4) 运行

注意用管理员权限来启动 freeSWITCH。

打开管理员权限的 cmd ，切换到 freeSWITCH 安装目录下，运行 FreeSwitchConsole.exe 。

启动完毕后，freeSWITCH会进入命令交互模式，可以直接输入命令。使用下列命令验证是否启动正常：

    version ，显示版本
    show codecs ，显示编解码器
    sofia status profile internal ，查看
    shutdown ，退出
    help ，显示帮助

5）验证端口

启动后，TCP 5060、UDP 5060 、TCP 5066 、TCP 7433 这几个端口应该被监听。

可以使用下面命令：

netstat -an | find "506"

netstat -an | find "7433"

    1
    2
    3

6）语音电话测试


