using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;

namespace MagellanTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly string connectionString = "Your PostgreSQL Connection String";

        // Endpoint to create a new record in the item table
        [HttpPost]
        public IActionResult CreateItem(Item newItem)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO item (item_name, parent_item, cost, req_date) VALUES (@itemName, @parentItem, @cost, @reqDate) RETURNING id";
                        command.Parameters.AddWithValue("@itemName", newItem.ItemName);
                        command.Parameters.AddWithValue("@parentItem", newItem.ParentItem);
                        command.Parameters.AddWithValue("@cost", newItem.Cost);
                        command.Parameters.AddWithValue("@reqDate", newItem.ReqDate);
                        var id = command.ExecuteScalar();
                        return Ok(id);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Endpoint to query the item table by ID
        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM item WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var item = new Item
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    ItemName = reader["item_name"].ToString(),
                                    ParentItem = reader["parent_item"] != DBNull.Value ? (int?)Convert.ToInt32(reader["parent_item"]) : null,
                                    Cost = Convert.ToInt32(reader["cost"]),
                                    ReqDate = Convert.ToDateTime(reader["req_date"])
                                };
                                return Ok(item);
                            }
                            else
                            {
                                return NotFound("Item not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Endpoint to call the Get_Total_Cost function
        [HttpGet("TotalCost")]
        public IActionResult GetTotalCost(string itemName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT Get_Total_Cost(@itemName)";
                        command.Parameters.AddWithValue("@itemName", itemName);
                        var result = command.ExecuteScalar();
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
