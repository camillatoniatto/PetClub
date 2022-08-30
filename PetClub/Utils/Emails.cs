using Microsoft.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClub.Utils
{
    public static class Emails
    {
        //public static string PasswordReset(string link, string name)
        //{
        //    object[] args = new object[] { link, name };

        //    string body = String.Format(
        //        @"<!DOCTYPE html>
        //        <html lang=""pt-br"">
               

        //        <head>
        //            <meta charset=""UTF-8"">
        //            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        //            <title>Email</title>
        //            <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Inter"">
        //        </head>

        //        <body>
        //            <div class=""row"">
        //                <div style=""text-align: center; padding-top: 50px; max-width: 600px;"">
        //                    <img src=""https://datletica.s3.us-east-2.amazonaws.com/logo_datletica.png"" width=""100"" height=""100"" style=""margin-bottom: 20px"">
        //                    <p style=""font-size: 20px; font-weight: 500; color: #5d5d5d; font-family: 'Inter';"">Oi {1}, você esqueceu sua senha?</p>
        //                    <p style=""font-size: 20px; font-weight: 500; color: #5d5d5d; padding-bottom: 25px;font-family: 'Inter';"">Não tem problema, só clicar no botão abaixo.</p>
            
        //                    <a href=""{0}"" style=""color: #fff;
        //                        background-color: #2c0c3c;
        //                        border-color: #2c0c3c;
        //                        display: inline-block;
        //                        font-weight: 400;
        //                        text-align: center;
        //                        white-space: nowrap;
        //                        vertical-align: middle;
        //                        -webkit-user-select: none;
        //                        -moz-user-select: none;
        //                        -ms-user-select: none;
        //                        user-select: none;
        //                        border: 1px solid transparent;
        //                        padding: 10px 38px;
        //                        font-size: 1rem;
        //                        line-height: 1.5;
        //                        border-radius: 0.25rem;
        //                        transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        //                        text-decoration: none;""> Resetar sua senha </a>

        //                    <div style=""text-align: left; padding-top: 45px;"">
        //                        <hr />
        //                        <p style=""font-size: 13px; color: #7d7d7d; font-family: 'Inter';"">Este email foi enviado a você pela DATLETICA. Caso não tenha
        //                            pedido para resetar sua senha, por favor, entre em contato imediatamente.</p>
        //                    </div>
        //                </div>
        //            </div>
        //        </body>

        //        </html>
        //        ", args);
        //    return body;
        //}

        //public static string PasswordResetConfirmed(string name)
        //{
        //    object[] args = new object[] { name };

        //    string body = String.Format(
        //        @"<!DOCTYPE html>
        //        <html lang=""pt-br"">
               

        //        <head>
        //            <meta charset=""UTF-8"">
        //            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        //            <title>Email</title>
        //            <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Inter"">
        //        </head>

        //        <body>
        //            <div class=""row"">
        //                <div style=""text-align: center; padding-top: 50px; max-width: 600px;"">
        //                    <img src=""https://datletica.s3.us-east-2.amazonaws.com/logo_datletica.png"" width=""100"" height=""100"" style=""margin-bottom: 20px"">
        //                    <p style=""font-size: 20px; font-weight: 500; color: #5d5d5d; font-family: 'Inter';"">Oi {0}, sua senha foi alterada com sucesso!</p>


        //                    <div style=""text-align: left; padding-top: 45px;"">
        //                        <hr />
        //                        <p style=""font-size: 13px; color: #7d7d7d; font-family: 'Inter';"">Este email foi enviado a você pela DATLETICA. Caso não tenha
        //                            pedido para resetar sua senha, por favor, entre em contato imediatamente.</p>
        //                    </div>
        //                </div>
        //            </div>
        //        </body>

        //        </html>
        //        ", args);
        //    return body;
        //}
    }
}
