# 针对开发者使用
#<font color = PURPLE>AR线索调查器</font>
###Version 0.0.1
###张子健
##1、分工
###1.1客户端应用程序逻辑
###1.2客户端UI界面交互
###1.3游戏创意及线索设计
###1.4服务器搭建及开发（挂起）
##2、技术环境配置
###2.1 Unity
####版本要求 Unity 5.3以上，Personal版本，除了逻辑设置外，所有人需要安装。
###2.2 安卓SDK
####版本要求 r24.4.2 只需下载安卓SDK，不需下Studio，用于项目发布到安卓端，独立开发的电脑需要下载。
###[安卓SDK环境配置](#AndroidSDKInstall)
###2.3 Xcode
####需MacOS系统，Unity发布到MacOS端，发布到Apple Store
###2.4 Remote
####手机调试APP，安卓端MAC端都有，在手机下载后，用USB线连接到电脑，Play模式下可在真机调试
###[Remote安装使用](#RemoteInstall)
###2.5 Vuforia 高通AR
####依赖包已经集成到git项目上,使用公用账号开发
>###账号：
>###密码：
>###License名：
>###Database名：

###[Vuforia官网](https://developer.vuforia.com/)
###[API文档](https://library.vuforia.com/reference/api/unity/index.html)
###[WebServiceAPI](https://library.vuforia.com/articles/Training/Using-the-VWS-API)
###[基础教程](https://library.vuforia.com/getting-started)
##3、脑图
##4、独立开发对接API
###4.1安卓设备传感器在Unity中调用方法
####4.1.1下载Unity插件包
###[AndroidSensor.unitypackage by ZhangZijian]()
####4.1.2导入插件包到Unity工程中
> 注：github上的项目包已经集成好，不需再次导入

> 如果Unity工程中Assets/Plugins/Android/AndroidManifest.xml文件已存在，请将

	<activity android:name="com.tongji.hei.androidinputpackage.MainActivity">
   		<intent-filter>
   			<action android:name="com.tongji.hei.androidinputpackage.android.intent.action.MAIN" />
   			<category android:name="com.tongji.hei.androidinputpackage.android.intent.category.LAUNCHER" />
		</intent-filter>
   		<meta-data android:name="com.tongji.hei.androidinputpackage.unityplayer.UnityActivity" android:value="true" />
  	 </activity>

> 	加入到 application标签中,并仅导入.arr文件

####4.1.3Unity中调用Java API方法：
	private AndroidJavaClass _class;
    private AndroidJavaObject _obj;
	_class = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	_obj = _class.GetStatic<AndroidJavaObject>("currentActivity");
	
	//调用
	T result = _obj.Call<T>(method_name as string,parameters)
####4.1.4安卓传感器API
预设常量：SENSOR.TYPE

	#define SENSOR_TYPE_ACCELEROMETER       1 //加速度
	#define SENSOR_TYPE_MAGNETIC_FIELD      2 //磁力
	#define SENSOR_TYPE_ORIENTATION         3 //方向
	#define SENSOR_TYPE_GYROSCOPE           4 //陀螺仪
	#define SENSOR_TYPE_LIGHT               5 //光线感应
	#define SENSOR_TYPE_PRESSURE            6 //压力
	#define SENSOR_TYPE_TEMPERATURE         7 //温度 
	#define SENSOR_TYPE_PROXIMITY           8 //接近
	#define SENSOR_TYPE_GRAVITY             9 //重力
	#define SENSOR_TYPE_LINEAR_ACCELERATION 10//线性加速度
	#define SENSOR_TYPE_ROTATION_VECTOR     11//旋转矢量

a.   
>boolean RegisterSensor(int sensor_type)

>功能：启用传感器

>参数：int类型，代表传感器种类

>返回值：bool 代表是否成功

b.
>boolean UnRegisterSensor(int sensor_type)

>功能：停用传感器

>参数：int类型，代表传感器种类

>返回值：bool 代表是否成功
	
c.
>SensorMessage getSensorMessage(int sensor_type)

>功能：获取传感器值

>SensorMessage： int type字段 float[] valus字段
