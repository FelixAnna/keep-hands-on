import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/contact.dart';

import '../controllers/groupChatDetailController.dart';
import 'group_chat_details_page.dart';

class GroupItem extends StatelessWidget {
  final GroupInfo member;
  GroupItem({
    required this.member,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () async {},
      child: Container(
        padding: EdgeInsets.only(left: 16, right: 16, top: 10, bottom: 10),
        child: Row(
          children: <Widget>[
            Expanded(
              child: Row(
                children: <Widget>[
                  CircleAvatar(
                    backgroundImage: NetworkImage(
                        "https://thumbs.dreamstime.com/z/little-cats-20284533.jpg"),
                    maxRadius: 30,
                  ),
                  SizedBox(
                    width: 16,
                  ),
                  Expanded(
                    child: Container(
                      color: Colors.transparent,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            this.member.Name,
                            style: TextStyle(fontSize: 16),
                          ),
                          SizedBox(
                            height: 6,
                          ),
                        ],
                      ),
                    ),
                  ),
                  IconButton(
                    icon: Icon(Icons.chat),
                    color: Colors.lightBlue,
                    onPressed: () async {
                      //Get.back();

                      var isRegistered =
                          Get.isRegistered<GroupChatDetailController>(
                              tag: this.member.GroupId);
                      if (!isRegistered) {
                        var chatController = GroupChatDetailController(
                          chatId: this.member.GroupId,
                          name: this.member.Name,
                        );
                        await chatController.initial();
                        chatController.subscribe(Get.find());
                        Get.put(chatController, tag: this.member.GroupId);
                      }

                      Get.to(() => GroupChatDetailsPage(
                            chatId: this.member.GroupId,
                            name: this.member.Name,
                          ));
                    },
                  )
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
