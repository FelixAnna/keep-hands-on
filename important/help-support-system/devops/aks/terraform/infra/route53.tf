# Configure DNS record in aws
data "aws_route53_zone" "selected" {
  name         = "metadlw.com"
}

resource "aws_route53_record" "api" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.backendDNS
  type    = "A"
  ttl     = 300
  records = [azurerm_public_ip.gwIp.ip_address]
}

resource "aws_route53_record" "idp" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.idpDNS
  type    = "A"
  ttl     = 300
  records = [azurerm_public_ip.gwIp.ip_address]
}

resource "aws_route53_record" "admin" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.AdminDNS
  type    = "A"
  ttl     = 300
  records = [date.azurerm_linux_web_app.cmsapp.default_hostname]
}

resource "aws_route53_record" "admin_asuid" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = "asuid.${var.AdminDNS}"
  type    = "TXT"
  ttl     = 300
  records = [data.azurerm_linux_web_app.cmsapp.custom_domain_verification_id]
}