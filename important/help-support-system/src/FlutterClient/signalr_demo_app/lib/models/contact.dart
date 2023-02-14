class Contact {
  String UserId;
  List<GroupInfo> Groups;
  List<MemberInfo> Friends;

  Contact(this.UserId, this.Groups, this.Friends);
}

class MemberInfo {
  String UserId;
  String NickName;
  String AvatarUrl;
  String Email;
  int TenantId;
  MemberInfo(
      {required this.UserId,
      required this.NickName,
      required this.AvatarUrl,
      required this.Email,
      required this.TenantId});

  MemberInfo.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        NickName = json['nickName'],
        AvatarUrl = json['avatarUrl'] ?? "",
        Email = json['email'],
        TenantId = json['tenantId'] ?? -1;

  Map<String, dynamic> toJson() => {
        'userId': this.UserId,
        'nickName': this.NickName,
        'avatarUrl': this.AvatarUrl,
        'email': this.Email,
        'tenantId': this.TenantId
      };
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

class ColleagueInfo extends MemberInfo {
  bool IsFriend;

  ColleagueInfo(
      {required int TenantId,
      required bool IsFriend,
      required String UserId,
      required String NickName,
      required String AvatarUrl,
      required String Email})
      : this.IsFriend = IsFriend,
        super(
            UserId: UserId,
            NickName: NickName,
            AvatarUrl: AvatarUrl,
            Email: Email,
            TenantId: TenantId);

  ColleagueInfo.fromJson(Map<String, dynamic> json)
      : IsFriend = json['isFriend'],
        super.fromJson(json);

  Map<String, dynamic> toJson() =>
      {'isFriend': this.IsFriend, ...super.toJson()};
}
