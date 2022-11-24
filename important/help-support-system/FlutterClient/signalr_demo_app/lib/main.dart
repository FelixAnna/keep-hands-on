import 'dart:io';

import 'package:flutter/material.dart';
import 'package:signalr_demo_app/utils/auth.dart';
import 'package:signalr_demo_app/utils/chart_hub_util.dart';
import 'package:signalr_demo_app/utils/config_util.dart';
import 'package:signalr_demo_app/widgets/chat_container.dart';
import 'package:signalr_demo_app/widgets/login.dart';
import 'my_http_overrides.dart';

void main() async {
  HttpOverrides.global = new MyHttpOverrides();
  WidgetsFlutterBinding.ensureInitialized();

  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {


    return MaterialApp(
        title: 'Flutter Demo',
        theme: ThemeData(
          primarySwatch: Colors.blue,
        ),
        home: //Auth.getToken() == ''
        //    ?
        MyHomePage(title: 'Flutter Demo Home Page')
          //  : Login() // MyHomePage(title: 'Flutter Demo Home Page'),
        );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key? key, required this.title}) : super(key: key);

  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initSignalR();
  }

  _initSignalR() async{
      await ChatHubUtil.initHubConnection(listeningMessage: (List<Object?>? listMessages)=>{});

      ChatHubUtil.startConnect(callback: () => Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => const Login())));
    }


  Future<void> _incrementCounter() async {
    setState(() {
      _counter++;
    });
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
                                  Navigator.pop(context);
                                },
                              ),
                              TextButton(
                                child: Text("Logout"),
                                onPressed: () {
                                  Auth.logout().then((value) => Navigator.pushReplacement(
                                      context,
                                      MaterialPageRoute(builder: (context) => const Login())));
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
      body: ChatContainer(),
      floatingActionButton: FloatingActionButton(
        onPressed: _incrementCounter,
        tooltip: 'Increment',
        child: Icon(Icons.add),
      ), // This trailing comma makes auto-formatting nicer for build methods.
      bottomNavigationBar: BottomNavigationBar(
        selectedItemColor: Colors.red,
        unselectedItemColor: Colors.grey.shade600,
        selectedLabelStyle: TextStyle(fontWeight: FontWeight.w600),
        unselectedLabelStyle: TextStyle(fontWeight: FontWeight.w600),
        type: BottomNavigationBarType.fixed,
        items: [
          BottomNavigationBarItem(
            icon: Icon(Icons.message),
            label: "Chats",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.group_work),
            label: "Channels",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.account_box),
            label: "Profile",
          ),
        ],
      ),
    );
  }
}
