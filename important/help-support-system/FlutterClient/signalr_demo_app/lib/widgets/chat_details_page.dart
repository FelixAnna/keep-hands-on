import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/chatDetailController.dart';

class ChatDetailsPage extends StatelessWidget {
  final String chatId;
  final String name;
  final String avatarUrl;
  ChatDetailsPage(
      {required this.chatId, required this.name, required this.avatarUrl});

  @override
  Widget build(BuildContext context) {
    WidgetsBinding.instance.addPostFrameCallback((_) =>
        Get.find<ChatDetailController>(tag: this.chatId).scrollToBottom());
    return Scaffold(
      appBar: AppBar(
        elevation: 0,
        automaticallyImplyLeading: false,
        backgroundColor: Colors.white,
        flexibleSpace: SafeArea(
          child: Container(
            padding: EdgeInsets.only(right: 16),
            child: Row(
              children: <Widget>[
                IconButton(
                  onPressed: () {
                    Get.back();
                  },
                  icon: Icon(
                    Icons.arrow_back,
                    color: Colors.black,
                  ),
                ),
                SizedBox(
                  width: 2,
                ),
                CircleAvatar(
                  backgroundImage: NetworkImage(avatarUrl),
                  maxRadius: 20,
                ),
                SizedBox(
                  width: 12,
                ),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      Text(
                        this.name,
                        style: TextStyle(
                            fontSize: 16, fontWeight: FontWeight.w600),
                      ),
                      SizedBox(
                        height: 6,
                      ),
                      Text(
                        "Online",
                        style: TextStyle(
                            color: Colors.grey.shade600, fontSize: 13),
                      ),
                    ],
                  ),
                ),
                Icon(
                  Icons.settings,
                  color: Colors.black54,
                ),
              ],
            ),
          ),
        ),
      ),
      body: Stack(
        children: <Widget>[
          GetX<ChatDetailController>(
            init: Get.find<ChatDetailController>(tag: this.chatId),
            builder: (_) {
              return ListView.builder(
                itemCount: _.messages.length,
                shrinkWrap: true,
                padding: EdgeInsets.only(top: 10, bottom: 70),
                physics: AlwaysScrollableScrollPhysics(),
                controller: _.scrollController,
                itemBuilder: (context, index) {
                  var isTargetMsg = _.messages[index].messageType == "receiver";
                  return Container(
                    padding: EdgeInsets.only(
                        left: 14, right: 14, top: 10, bottom: 10),
                    child: Align(
                      alignment: (isTargetMsg
                          ? Alignment.topLeft
                          : Alignment.topRight),
                      child: Container(
                        padding: EdgeInsets.all(8),
                        child: Container(
                          child: Row(
                            mainAxisAlignment: isTargetMsg
                                ? MainAxisAlignment.start
                                : MainAxisAlignment.end,
                            children: [
                              isTargetMsg
                                  ? Tooltip(
                                      message: name,
                                      triggerMode: TooltipTriggerMode.tap,
                                      child: CircleAvatar(
                                        backgroundImage:
                                            NetworkImage(avatarUrl),
                                      ),
                                    )
                                  : Container(),
                              Container(
                                decoration: BoxDecoration(
                                  borderRadius: BorderRadius.circular(8),
                                  color: (isTargetMsg
                                      ? Colors.grey.shade200
                                      : Colors.blue[200]),
                                ),
                                padding: EdgeInsets.all(16),
                                child: Text(
                                  _.messages[index].messageContent,
                                  style: TextStyle(fontSize: 16),
                                ),
                              ),
                              isTargetMsg
                                  ? Container()
                                  : Tooltip(
                                      message: _.Profile.NickName,
                                      triggerMode: TooltipTriggerMode.tap,
                                      child: CircleAvatar(
                                        backgroundImage:
                                            NetworkImage(_.Profile.AvatarUrl),
                                      ),
                                    ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  );
                },
              );
            },
          ),
          Align(
            alignment: Alignment.bottomLeft,
            child: Container(
              padding: EdgeInsets.only(left: 10, bottom: 10, top: 10),
              height: 60,
              width: double.infinity,
              color: Colors.white,
              child: Row(
                children: <Widget>[
                  GestureDetector(
                    onTap: () {},
                    child: Container(
                      height: 30,
                      width: 30,
                      decoration: BoxDecoration(
                        color: Colors.lightBlue,
                        borderRadius: BorderRadius.circular(30),
                      ),
                      child: Icon(
                        Icons.add,
                        color: Colors.white,
                        size: 20,
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 15,
                  ),
                  Expanded(
                      child: TextField(
                    controller: Get.find<ChatDetailController>(tag: this.chatId)
                        .messageController,
                    decoration: InputDecoration(
                        hintText: "Write message...",
                        hintStyle: TextStyle(color: Colors.black54),
                        border: InputBorder.none),
                  )),
                  SizedBox(
                    width: 15,
                  ),
                  FloatingActionButton(
                    onPressed: () {
                      Get.find<ChatDetailController>(tag: this.chatId)
                          .sendMsg();
                    },
                    child: Icon(
                      Icons.send,
                      color: Colors.white,
                      size: 18,
                    ),
                    backgroundColor: Colors.blue,
                    elevation: 0,
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
