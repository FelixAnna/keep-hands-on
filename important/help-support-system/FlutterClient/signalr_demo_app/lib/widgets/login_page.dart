import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/services/auth_service.dart';
import 'package:url_launcher/url_launcher.dart';

import '../utils/localstorage_service.dart';

class LoginPage extends StatefulWidget {
  final AuthService authService;
  const LoginPage({Key? key, required this.authService}) : super(key: key);

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  var usernameController;
  final passwordController = TextEditingController();

  @override
  void initState() {
    // TODO: implement initState
    _init();
    super.initState();
  }

  _init() async {
    var remText =
        await LocalStorageService.get(LocalStorageService.REMEMBER_USERNAME);
    setState(() {
      usernameController = TextEditingController(text: remText);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        title: Text("Login Page"),
      ),
      body: SingleChildScrollView(
        child: Column(
          children: <Widget>[
            Padding(
              padding: const EdgeInsets.only(top: 60.0),
              child: Center(
                child: Container(width: 200, height: 150),
              ),
            ),
            Padding(
              //padding: const EdgeInsets.only(left:15.0,right: 15.0,top:0,bottom: 0),
              padding: EdgeInsets.symmetric(horizontal: 15),
              child: TextField(
                controller: usernameController,
                decoration: InputDecoration(
                    border: OutlineInputBorder(),
                    labelText: 'Email',
                    hintText: 'Enter valid email id as abc@gmail.com'),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(
                  left: 15.0, right: 15.0, top: 15, bottom: 0),
              //padding: EdgeInsets.symmetric(horizontal: 15),
              child: TextField(
                controller: passwordController,
                obscureText: true,
                decoration: InputDecoration(
                    border: OutlineInputBorder(),
                    labelText: 'Password',
                    hintText: 'Enter secure password'),
              ),
            ),
            TextButton(
              onPressed: () {
                launchUrl(Uri.https(widget.authService.env.idpAuthority,
                    "Identity/Account/ForgotPassword"));
              },
              child: Text(
                'Forgot Password',
                style: TextStyle(color: Colors.blue, fontSize: 15),
              ),
            ),
            Container(
              height: 50,
              width: 250,
              decoration: BoxDecoration(
                  color: Colors.blue, borderRadius: BorderRadius.circular(20)),
              child: TextButton(
                //onHover:(value) => ,
                onPressed: () {
                  // here to login
                  widget.authService
                      .login(usernameController.text, passwordController.text)
                      .then(
                    (isSucceed) async {
                      if (isSucceed) {
                        await LocalStorageService.save(
                            LocalStorageService.REMEMBER_USERNAME,
                            usernameController.text);
                        Get.back();
                      } else {
                        //failed action
                      }
                    },
                  );
                },
                child: Text(
                  'Login',
                  style: TextStyle(color: Colors.white, fontSize: 25),
                ),
              ),
            ),
            SizedBox(
              height: 130,
            ),
            TextButton(
              onPressed: () {
                launchUrl(
                    Uri.https(widget.authService.env.idpAuthority,
                        "Identity/Account/Register"),
                    mode: LaunchMode.inAppWebView);
              },
              child: Text(
                'Create Account',
                style: TextStyle(color: Colors.blue, fontSize: 15),
              ),
            ),
            //Text('New User? Create Account')
          ],
        ),
      ),
    );
  }
}
