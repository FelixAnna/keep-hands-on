import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/profileController.dart';

class ProfilePage extends GetWidget<ProfileController> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(32),
        child: Row(
          children: [
            Expanded(
                child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                /*2*/
                Container(
                  padding: const EdgeInsets.only(bottom: 8),
                  child: Obx(() => Text(
                        controller.UserInfo.value.UserName,
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                        ),
                      )),
                ),
                Obx(() => Text(
                      controller.UserInfo.value.Email,
                      style: TextStyle(
                        color: Colors.grey[500],
                      ),
                    )),
                Obx(() => Text(
                      controller.UserInfo.value.UserId,
                      style: TextStyle(
                        color: Colors.grey[500],
                      ),
                    )),
              ],
            )),
            /*3*/
            Icon(
              Icons.star,
              color: Colors.amber,
            ),
            const Text('99'),
          ],
        ),
      ),
    );
  }
}
