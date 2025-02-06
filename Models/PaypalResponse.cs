using System.Collections.Generic;

public class PaypalResponse
{
    public string id { get; set; }
    public string status { get; set; }
    public string create_time { get; set; }
    public string update_time { get; set; }
    public Comprador comprador { get; set; }
    public List<UnidadCompra> purchase_units { get; set; }
}

public class Comprador
{
    public string payer_id { get; set; }
    public string email_address { get; set; }
    public Nombre nombre { get; set; }
    public Direccion? direccion { get; set; }
}

public class Nombre
{
    public string given_name { get; set; }
    public string surname { get; set; }
}

public class Direccion
{
    public string country_code { get; set; }
}

public class UnidadCompra
{
    public Monto monto { get; set; }
    public Beneficiario payee { get; set; }
}

public class Monto
{
    public string currency_code { get; set; }
    public string valor { get; set; }
}

public class Beneficiario
{
    public string email_address { get; set; }
    public string merchant_id { get; set; }
}
