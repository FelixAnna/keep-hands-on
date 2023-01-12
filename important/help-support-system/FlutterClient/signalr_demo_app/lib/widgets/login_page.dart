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
                    labelText: 'Username',
                    hintText: 'Enter your username'),
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
              child: GetX<AuthController>(
                builder: (_) {
                  return TextButton(
                    onPressed: () async {
                      if (controller.isLoading.value) {
                        return;
                      }
                      await controller.login(true);
                      if (controller.Token == '') {
                        showDialog(
                          context: context,
                          builder: (ctx) => AlertDialog(
                            title: const Text("Login Failed"),
                            content: const Text(
                                "Please check your network, username and password."),
                            actions: <Widget>[
                              TextButton(
                                onPressed: () {
                                  Navigator.of(ctx).pop();
                                },
                                child: Container(
                                  padding: const EdgeInsets.all(14),
                                  child: const Text("Okay"),
                                ),
                              ),
                            ],
                          ),
                        );
                        return;
                      }

                      Get.back();

                      await Get.find<ChatContainerController>().initial();
                      await Get.find<ProfileController>().initial();
                      //failed action
                    },
                    child: controller.isLoading.value
                        ? Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: const [
                              Text(
                                'Loading...',
                                style: TextStyle(
                                  fontSize: 20,
                                  color: Colors.white,
                                ),
                              ),
                              SizedBox(
                                width: 10,
                              ),
                              CircularProgressIndicator(
                                color: Colors.white,
                              ),
                            ],
                          )
                        : Text(
                            'Login',
                            style: TextStyle(
                              color: Colors.white,
                              fontSize: 25,
                            ),
                          ),
                  );
                },
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
