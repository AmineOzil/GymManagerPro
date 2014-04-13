﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;

namespace DataLayer
{
    class Membership
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int Plan { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public double price { get; set; }

    }

    class Memberships
    {


        // inserts a new membership in the db
        public static int NewMembership(Membership membership)
        {
            string query = "INSERT INTO Memberships (Member, [Plan], StartDate, EndDate) VALUES (@memberid, @planid, @startDate, @endDate)";

            using (SqlCeConnection con = DB.GetSqlCeConnection())
            {
                SqlCeCommand cmd = new SqlCeCommand(query, con);
                cmd.Parameters.AddWithValue("@memberid", membership.Id);
                cmd.Parameters.AddWithValue("@planid", membership.Plan);
                cmd.Parameters.AddWithValue("@StartDate", membership.start);
                cmd.Parameters.AddWithValue("@EndDate", membership.end);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }

        }


        public static int UpdateMembership(Membership membership)
        {
           string query = "UPDATE Memberships SET [Plan] = @planid, StartDate = @startdate, EndDate = @enddate WHERE Member = @memberid AND Id = @membershipid";
            using (SqlCeConnection con = DB.GetSqlCeConnection())
            {
                SqlCeCommand cmd = new SqlCeCommand(query, con);
                cmd.Parameters.AddWithValue("@planid", membership.Plan);
                cmd.Parameters.AddWithValue("@startdate", membership.start);
                cmd.Parameters.AddWithValue("@enddate", membership.end);
                cmd.Parameters.AddWithValue("@memberid", membership.MemberId);
                cmd.Parameters.AddWithValue("@membershipid", membership.Id);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }


        /// <summary>
        /// deletes the specified membership
        /// </summary>
        /// <param name="id"></param>
        /// <returns>number of affected rows</returns>
        public static int DeleteMembership(int id)
        {
            string query = "DELETE FROM Memberships WHERE Id = @id";
            using (SqlCeConnection con = DB.GetSqlCeConnection())
            {
                SqlCeCommand cmd = new SqlCeCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
        }

        /// <summary>
        /// retrieves membership details for a given member
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static DataTable GetMembershipByMemberId(int memberID)
        {
            DataTable table = new DataTable();
            SqlCeDataAdapter da = null;

            string query = "SELECT        Plans.Id, Plans.Name, Memberships.StartDate, Memberships.EndDate, Plans.Price " +
                           "FROM          Plans INNER JOIN " +
                           "                 Memberships INNER JOIN " +
                           "                  Members ON Memberships.Member = Members.Id ON Plans.Id = Memberships.[Plan] " +
                           "WHERE        (Members.Id = @memberId)";

            using (SqlCeConnection con = DB.GetSqlCeConnection())
            {
                SqlCeCommand cmd = new SqlCeCommand(query, con);
                cmd.Parameters.AddWithValue("@memberId", memberID);

                da = new SqlCeDataAdapter(cmd);
                da.Fill(table);
            }

            return table;
        }



    }
}
