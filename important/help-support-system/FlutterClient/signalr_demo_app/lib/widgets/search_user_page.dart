import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/widgets/user_item.dart';
import '../controllers/searchUserController.dart';

class SearchUserPage extends GetWidget<SearchUserController> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        physics: BouncingScrollPhysics(),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            SafeArea(
              child: Padding(
                padding: EdgeInsets.only(left: 16, right: 16, top: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: <Widget>[
                    Text(
                      "Search User",
                      style:
                          TextStyle(fontSize: 32, fontWeight: FontWeight.bold),
                    )
                  ],
                ),
              ),
            ),
            Obx(() => ListView.builder(
                  itemCount: controller.Users.length,
                  shrinkWrap: true,
                  padding: EdgeInsets.only(top: 16),
                  physics: NeverScrollableScrollPhysics(),
                  itemBuilder: (context, index) {
                    var info = controller.Users[index];
                    return Obx(() => UserItem(
                          member: info,
                        ));
                  },
                )),
          ],
        ),
      ),
    );
  }

  String getTime(DateTime? time) {
    if (time == null) {
      return "Never";
    }

    var now = DateTime.now().toUtc();
    var diff = now.difference(time).inMinutes;
    if (diff <= 2) {
      return "Just now";
    } else if (diff < 60) {
      return "${diff} mins ago";
    } else if (diff < 1440) {
      return "${(diff / 60).round()} hour(s) ago";
    } else {
      return time.toIso8601String();
    }
  }
}
