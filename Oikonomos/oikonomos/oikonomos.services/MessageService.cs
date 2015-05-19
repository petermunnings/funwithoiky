using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces.Messages;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRecepientRepository _messageRecepientRepository;

        public MessageService(IMessageRecepientRepository messageRecepientRepository)
        {
            _messageRecepientRepository = messageRecepientRepository;
        }

        public JqGridData GetMessageStatuses(Person currentPerson, JqGridRequest request)
        {
            var messages = new List<MessageWithStatusViewModel>();
            if (currentPerson.HasPermission(Permissions.SystemAdministrator))
            {
                messages = _messageRecepientRepository.FetchMessagesWithStatuses().ToList();
            }
            var totalRecords = messages.Count();

            switch (request.sidx)
            {
                case "Subject":
                {
                    if (request.sord.ToLower() == "asc")
                    {
                        messages = messages.OrderBy(g => g.Subject)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    else
                    {
                        messages = messages
                            .OrderByDescending(g => g.Subject)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    break;
                }
                case "Sent":
                {
                    if (request.sord.ToLower() == "asc")
                    {
                        messages = messages.OrderBy(g => g.Sent)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    else
                    {
                        messages = messages
                            .OrderByDescending(g => g.Sent)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    break;
                }
                case "Status":
                {
                    if (request.sord.ToLower() == "asc")
                    {
                        messages = messages.OrderBy(g => g.Status)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    else
                    {
                        messages = messages
                            .OrderByDescending(g => g.Status)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    break;
                }
                case "From":
                {
                    if (request.sord.ToLower() == "asc")
                    {
                        messages = messages.OrderBy(g => g.From)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    else
                    {
                        messages = messages
                            .OrderByDescending(g => g.From)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    break;
                }
                case "To":
                {
                    if (request.sord.ToLower() == "asc")
                    {
                        messages = messages.OrderBy(g => g.To)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    else
                    {
                        messages = messages
                            .OrderByDescending(g => g.To)
                            .Skip((request.page - 1)*request.rows)
                            .Take(request.rows)
                            .ToList();
                    }
                    break;
                }
            }


            var messagesGridData = new JqGridData()
            {
                total = (int) Math.Ceiling((float) totalRecords/(float) request.rows),
                page = request.page,
                records = totalRecords,
                rows = (from g in messages
                    select new JqGridRow
                    {
                        id = g.MessageRecepientId,
                        cell = new string[]
                        {
                            g.MessageRecepientId,
                            g.Sent.ToString("yyyy-MM-dd"),
                            g.Status,
                            g.From,
                            g.To,
                            g.Subject,
                            g.StatusDetail
                        }
                    }).ToArray()
            };
            return messagesGridData;
        }
    }
}
