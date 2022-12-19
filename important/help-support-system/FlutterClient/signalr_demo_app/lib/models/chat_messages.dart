import 'package:signalr_demo_app/models/msg_dto.dart';

class ChatMessages {
  late String OwnerId;
  late String ChatId;
  late String LatestMsge;
  late int UnreadCount;
  late DateTime? Time;
  late List<MsgDto> Messages;

  ChatMessages(
      {required this.OwnerId,
      required this.ChatId,
      required this.LatestMsge,
      required this.UnreadCount,
      required this.Time,
      required this.Messages});

  Map<String, dynamic> toMap() {
    return {
      'ownerId': OwnerId,
      'chatId': ChatId,
      'latestMsge': LatestMsge,
      'unreadCount': UnreadCount,
      'time': Time?.toUtc().toString(),
    };
  }
}
