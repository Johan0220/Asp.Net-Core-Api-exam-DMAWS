using examAzure.Entities;
using examAzure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace examAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageOrderController : ControllerBase
    {

        private readonly MyDbContext _context;

        public ManageOrderController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("order/get")]
        public ActionResult Get()
        {
            var order = _context.OrderTbl.ToList();

            if ( order.Count == 0 )
            {
                return BadRequest();
            }

            return Ok( new
            {
                status = 200,
                metadata = order
            });
        }


        [HttpPut]
        [Route("order/update")]
        public ActionResult updateOrder(modelOrder order_u)
        {
            var order = _context.OrderTbl.FirstOrDefault( o => o.itemCode == order_u.itemCode);

            if ( order == null )
            {
                return NotFound("not found the data");
            }

            order.ItemName = order_u.itemName;
            order.ItemQty = order_u.quantity;
            order.OrderAddress = order_u.address;
            order.PhoneNumber = order_u.phoneNumber;

            _context.SaveChanges();
            return Ok("update success");
        }

        [HttpPost]
        [Route("order/post")]
        public async Task<ActionResult> post(modelOrder2 order_u)
        {
            var orde_new = new OrderTbl()
            {
                itemCode = await RandomString(),
                ItemName = order_u.itemName,
                ItemQty = order_u.quantity,
                OrderDelivery = (DateTime.Now).ToString(),
                OrderAddress = order_u.address,
                PhoneNumber = order_u.phoneNumber
            };

            _context.Add(orde_new);
            _context.SaveChanges();
            return Ok( new
            {
                status = 200,
                message = "add success",
                metadata = orde_new
            } );
        }

        [HttpDelete]
        [Route("order/delete")]
        public ActionResult delete(string orderId)
        {
            var order = _context.OrderTbl.FirstOrDefault(o => o.itemCode == orderId);

            if (order == null)
            {
                return NotFound("fail pls try again");
            }
            _context.Remove(order);
            _context.SaveChanges();

            return Ok(new
            {
                status = 200,
                message = "delete success"
            });
        }



        private static async Task<string> RandomString(int length = 6)
        {
            Random random = new Random();
            string character = "ABCDEFGHIJKLMNOPQRSTUVWX";

            // task chỉ sử dụng trong trường hợp liên quan đến việc tính toán một việc nhất định ...
            // hoặc là những việc liên quan đến độ trễ của DB nhưng ko nhớ ví dụ 
            Task<string> task = Task.Run(() => new string(Enumerable.Repeat(character, length).Select(s => s[random.Next(s.Length)]).ToArray()));

            string result = await task;

            return result;
        }

    }
}
