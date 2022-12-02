class Contact {
  String UserId;
  List<GroupInfo> Groups;
  List<MemberInfo> Friends;

  Contact(this.UserId, this.Groups, this.Friends);
}

class MemberInfo {
  String UserId;
  String UserName;
  String Email;
  MemberInfo(
      {required this.UserId, required this.UserName, required this.Email});

  MemberInfo.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        UserName = json['userName'],
        Email = json['email'];

  Map<String, dynamic> toJson() =>
      {'userId': this.UserId, 'userName': this.UserName, 'email': this.Email};
}

class GroupInfo {
  String GroupId;
  String Name;
  GroupInfo({required this.GroupId, required this.Name});

  GroupInfo.fromJson(Map<String, dynamic> json)
      : GroupId = json['groupId'].toString(),
        Name = json['name'];

  Map<String, dynamic> toJson() => {
        'groupId': this.GroupId,
        'name': this.Name,
      };
}
