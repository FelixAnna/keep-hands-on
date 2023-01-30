import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/config/env.dart';
import 'package:signalr_demo_app/controllers/chatContainerController.dart';
import 'package:signalr_demo_app/controllers/searchUserController.dart';
import 'package:signalr_demo_app/services/auth_service.dart';
import 'package:signalr_demo_app/services/group_member_service.dart';
import 'package:signalr_demo_app/services/hub_service.dart';
import 'package:signalr_demo_app/services/message_service.dart';
import 'package:signalr_demo_app/services/message_storage_service.dart';
import 'package:signalr_demo_app/services/tenant_service.dart';
import 'package:signalr_demo_app/services/user_service.dart';
import 'package:signalr_demo_app/utils/authorized_client.dart';
import 'package:signalr_demo_app/widgets/channels_page.dart';
import 'package:signalr_demo_app/widgets/chat_container_page.dart';
import 'package:signalr_demo_app/widgets/login_page.dart';
import 'package:signalr_demo_app/widgets/profile_page.dart';
import 'controllers/authController.dart';
import 'controllers/profileController.dart';
import 'my_http_overrides.dart';
import 'package:http/http.dart' as http;

import 'services/auth_storage_service.dart';

Future<void> main() async {
  HttpOverrides.global = new MyHttpOverrides();
  WidgetsFlutterBinding.ensureInitialized();

  await dotenv.load(fileName: "env/.env");
  print('UserApiAddress: ${dotenv.get('userApiAddress')}');

  runApp(HSSApp());
}

class HSSApp extends StatelessWidget {
  final List<Widget> pages = [];

  late final LoginPage loginWidget;
  late final ChatContainerPage containerWidget;
  late final ProfilePage profileWidget;
  late final ChannelsPage channelsPage;

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    if (pages.isEmpty) {
      initDI();
      initWidget();
      initNavBar();
    }

    return GetMaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Help Support Chat',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(
        title: 'Help Support Chat Home Page',
        pages: pages,
      ),
      initialRoute: "/",
      getPages: [
        GetPage(
          name: "/login",
          page: () => loginWidget,
        ),
        GetPage(
          name: "/channels",
          page: () => channelsPage,
        ),
        GetPage(
          name: "/conversation",
          page: () => containerWidget,
        ),
      ],
    );
  }

  initDI() {
    var client = new AuthorizedClient(http.Client());
    IAppEnv envService = AppEnv();
    Get.put<IAppEnv>(envService, permanent: true);
    Get.put(AuthService(env: envService), permanent: true);
    Get.put(TenantService(env: envService), permanent: true);
    Get.put(MessageService(client, env: envService), permanent: true);
    Get.put(UserService(client, env: envService), permanent: true);
    Get.put(GroupMemberService(client, env: envService), permanent: true);

    Get.put(AuthStorageService(), permanent: true);
    Get.put(SearchUserController(), permanent: true);
    Get.put(HubService(env: envService), permanent: true);
    Get.put(MessageStorageService(), permanent: true);
    Get.put(ProfileController(), permanent: true);
    Get.put(AuthController(), permanent: true);
    Get.put(ChatContainerController(), permanent: true);
  }

  initWidget() {
    loginWidget = LoginPage();
    containerWidget = ChatContainerPage();
    profileWidget = ProfilePage();
    channelsPage = ChannelsPage();
  }

  initNavBar() {
    pages.add(containerWidget);
    pages.add(channelsPage);
    pages.add(profileWidget);
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({
    Key? key,
    required this.title,
    required this.pages,
  }) : super(key: key);

  final String title;
  final List<Widget> pages;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int currentPage = 0;
  late AuthController controller;

  @override
  void initState() {
    super.initState();
    controller = Get.find<AuthController>();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
        actions: <Widget>[
          IconButton(
            icon: Icon(
              Icons.logout,
              color: Colors.white,
            ),
            onPressed: () {
              showDialog(
                  builder: (ctxt) {
                    return AlertDialog(
                        title: Text("Logout"),
                        content: Column(
                          mainAxisAlignment: MainAxisAlignment.spaceAround,
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Text("Do you Really want to logout?"),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceAround,
                              children: [
                                TextButton(
                                  child: Text("Cancel"),
                                  onPressed: () {
                                    Get.back();
                                  },
                                ),
                                TextButton(
                                  child: Text("Logout"),
                                  onPressed: () {
                                    Navigator.pop(ctxt);
                                    controller.logout().then((value) => {
                                          Get.toNamed("/login"),
                                        });
                                  },
                                )
                              ],
                            ),
                          ],
                        ));
                  },
                  context: context);
            },
          )
        ],
      ),
      body: widget.pages[currentPage],
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          debugPrint("You clicked login");
        },
        tooltip: 'Increment',
        child: Icon(Icons.add),
      ), // This trailing comma makes auto-formatting nicer for build methods.
      bottomNavigationBar: BottomNavigationBar(
        selectedItemColor: Colors.blue,
        unselectedItemColor: Colors.grey.shade600,
        selectedLabelStyle: TextStyle(fontWeight: FontWeight.w900),
        unselectedLabelStyle: TextStyle(fontWeight: FontWeight.w600),
        onTap: (int index) {
          setState(() {
            currentPage = index;
          });
        },
        currentIndex: currentPage,
        type: BottomNavigationBarType.fixed,
        items: [
          BottomNavigationBarItem(
            icon: Icon(Icons.chat),
            label: "Chats",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.group_work),
            label: "Channels",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: "Profile",
          ),
        ],
      ),
    );
  }
}
