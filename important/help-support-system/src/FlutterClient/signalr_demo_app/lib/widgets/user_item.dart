import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/contact.dart';

import '../controllers/chatDetailController.dart';
import 'chat_details_page.dart';

class UserItem extends StatelessWidget {
  final ColleagueInfo member;
  UserItem({
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
                    backgroundImage: NetworkImage(this.member.AvatarUrl),
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
                            this.member.NickName,
                            style: TextStyle(fontSize: 16),
                          ),
                          SizedBox(
                            height: 6,
                          ),
                          !this.member.IsFriend
                              ? IconButton(
                                  icon: Icon(Icons.person_add),
                                  color: Colors.lightBlue,
                                  onPressed: () async {
                                    // TODO add relationship
                                    // TODO
                                    print(
                                        "add friend relationship is on the way...");
                                  },
                                )
                              : Container()
                        ],
                      ),
                    ),
                  ),
                  IconButton(
                    icon: Icon(this.member.IsFriend
                        ? Icons.chat
                        : Icons.chat_bubble_outline_rounded),
                    color: Colors.lightBlue,
                    onPressed: () async {
                      //Get.back();

                      var isRegistered = Get.isRegistered<ChatDetailController>(
                          tag: this.member.UserId);
                      if (!isRegistered) {
                        var chatController = ChatDetailController(
                          chatId: this.member.UserId,
                          name: this.member.NickName,
                        );
                        await chatController.initial();
                        chatController.subscribe(Get.find());
                        Get.put(chatController, tag: this.member.UserId);
                      }

                      Get.to(() => ChatDetailsPage(
                            chatId: this.member.UserId,
                            name: this.member.NickName,
                            avatarUrl: this.member.AvatarUrl,
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
