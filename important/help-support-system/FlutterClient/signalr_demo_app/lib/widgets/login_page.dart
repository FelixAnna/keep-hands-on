import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/authController.dart';
import 'package:signalr_demo_app/controllers/profileController.dart';
import 'package:url_launcher/url_launcher.dart';

import '../controllers/chatContainerController.dart';

class LoginPage extends GetWidget<AuthController> {
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
                controller: controller.UserNameEditor,
                decoration: InputDecoration(
                    border: OutlineInputBorder(),
                    labelText: 'User Name',
                    hintText: 'Enter your user name'),
              ),
            ),
            Padding(
                padding: const EdgeInsets.only(
                    left: 15.0, right: 15.0, top: 15, bottom: 0),
                //padding: EdgeInsets.symmetric(horizontal: 15),
                child: TextField(
                  controller: controller.PasswordEditor,
                  obscureText: true,
                  decoration: InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Password',
                      hintText: 'Enter secure password'),
                )),
            TextButton(
              onPressed: () {
                launchUrl(Uri.https(controller.IdpAuthority,
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
                onPressed: () async {
                  await controller.login(true);
                  await Get.find<ProfileController>().initial();
                  Get.back();
                  //failed action
                },
                child: Text(
                  'Login',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 25,
                  ),
                ),
              ),
            ),
            SizedBox(
              height: 130,
            ),
            TextButton(
              onPressed: () {
                launchUrl(
                    Uri.https(
                        controller.IdpAuthority, "Identity/Account/Register"),
                    mode: LaunchMode.inAppWebView);
              },
              child: Text(
                'Create Account',
                style: TextStyle(color: Colors.blue, fontSize: 15),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
