using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;


namespace lw.Network
{
	[Serializable]
	public class SerializableLinkedResource
	{
		private String contentID;

		private Uri contentLink;

		private Stream contentStream;

		private SerializableContentType serializedContentType;

		private TransferEncoding transferEncoding;

		/// <summary>
		/// Serializable Linked Resource
		/// </summary>
		/// <param name="linkedResource">Linked Resource that needs to be serialized</param>
		/// <returns>Serializable Linked Resource</returns>
		public static SerializableLinkedResource GetSerializableLinkedResource(LinkedResource linkedResource)
		{
			if (linkedResource == null) return null;

			var serializedLinkedResource = new SerializableLinkedResource { contentID = linkedResource.ContentId, contentLink = linkedResource.ContentLink };

			if (linkedResource.ContentStream != null)
			{
				var bytes = new byte[linkedResource.ContentStream.Length];
				linkedResource.ContentStream.Read(bytes, 0, bytes.Length);
				serializedLinkedResource.contentStream = new MemoryStream(bytes);
			}

			serializedLinkedResource.serializedContentType =
			  SerializableContentType.GetSerializableContentType(linkedResource.ContentType);
			serializedLinkedResource.transferEncoding = linkedResource.TransferEncoding;

			return serializedLinkedResource;

		}

		/// <summary>
		/// Gets original Linked Resource
		/// </summary>
		/// <returns>original linked resource</returns>
		public LinkedResource GetOriginalLinkedResource()
		{
			var linkedResource = new LinkedResource(contentStream)
			{
				ContentId = this.contentID,
				ContentLink = this.contentLink,
				ContentType = this.serializedContentType.GetOriginalContentType(),
				TransferEncoding = this.transferEncoding
			};


			return linkedResource;
		}
	}

	/// <summary>
	/// SerializableAlternateView
	/// </summary>
	[Serializable]
	public class SerializableAlternateView
	{
		private Uri baseUri;

		private String serializedContentId;

		private Stream serializedContentStream;

		private SerializableContentType serializedContentType;

		private readonly List<SerializableLinkedResource> serializedLinkedResources = new List<SerializableLinkedResource>();

		private TransferEncoding transferEncoding;

		/// <summary>
		/// Returns serializable alternate view object
		/// </summary>
		/// <param name="alternateView"></param>
		/// <returns></returns>
		public static SerializableAlternateView GetSerializableAlternateView(AlternateView alternateView)
		{
			if (alternateView == null) return null;

			var serializedAlternateView = new SerializableAlternateView { serializedContentId = alternateView.ContentId, baseUri = alternateView.BaseUri };



			if (alternateView.ContentStream != null)
			{
				var bytes = new byte[alternateView.ContentStream.Length];
				alternateView.ContentStream.Read(bytes, 0, bytes.Length);
				serializedAlternateView.serializedContentStream = new MemoryStream(bytes);
			}

			serializedAlternateView.serializedContentType =
			  SerializableContentType.GetSerializableContentType(alternateView.ContentType);

			foreach (var lr in alternateView.LinkedResources)
			{
				serializedAlternateView.serializedLinkedResources.Add(SerializableLinkedResource.GetSerializableLinkedResource(lr));
			}

			serializedAlternateView.transferEncoding = alternateView.TransferEncoding;

			return serializedAlternateView;
		}

		/// <summary>
		/// Gets original alternate view back
		/// </summary>
		/// <returns>original alternate view</returns>
		public AlternateView GetOriginalAlternateView()
		{
			var alternateView = new AlternateView(serializedContentStream)
			{
				ContentId = this.serializedContentId,
				BaseUri = this.baseUri,
				ContentType = this.serializedContentType.GetOriginalContentType()
			};

			foreach (var lr in serializedLinkedResources)
			{
				alternateView.LinkedResources.Add(lr.GetOriginalLinkedResource());
			}

			alternateView.TransferEncoding = transferEncoding;

			return alternateView;
		}
	}


	/// <summary>
	/// Serializable Mail Address
	/// </summary>
	[Serializable]
	public class SerializableMailAddress
	{
		private String address;

		private String displayName;

		/// <summary>
		/// Serializes mail address
		/// </summary>
		/// <param name="mailAddress">original mail address that needs to be serailized</param>
		/// <returns></returns>
		public static SerializableMailAddress GetSerializableMailAddress(MailAddress mailAddress)
		{
			if (mailAddress == null) return null;
			var serializedMailAddress = new SerializableMailAddress
			{
				displayName = mailAddress.DisplayName,
				address = mailAddress.Address
			};

			return serializedMailAddress;
		}

		/// <summary>
		/// Returns the original mail address
		/// </summary>
		/// <returns>original mail address</returns>
		public MailAddress GetOriginalMailAddress()
		{
			return new MailAddress(address, displayName);
		}
	}

	/// <summary>
	/// Serializable content disposition
	/// </summary>
	[Serializable]
	public class SerializableContentDisposition
	{
		private DateTime creationDate;

		private String dispositionType;

		private String fileName;

		private Boolean inline;

		private DateTime modificationDate;

		private SerializableCollection serializedColParameters;

		private DateTime readDate;

		private long size;

		/// <summary>
		/// Serailizes ontent disposition
		/// </summary>
		/// <param name="contentDisposition">content disposition to be serialized</param>
		/// <returns>serialized content disposition</returns>
		public static SerializableContentDisposition GetSerializableContentDisposition(ContentDisposition contentDisposition)
		{
			if (contentDisposition == null) return null;

			var serializedContentDisposition = new SerializableContentDisposition
			{
				creationDate = contentDisposition.CreationDate,
				dispositionType = contentDisposition.DispositionType,
				fileName = contentDisposition.FileName,
				inline = contentDisposition.Inline,
				modificationDate = contentDisposition.ModificationDate,
				serializedColParameters = SerializableCollection.GetSerializableCollection(contentDisposition.Parameters),
				readDate = contentDisposition.ReadDate,
				size = contentDisposition.Size
			};

			return serializedContentDisposition;
		}

		/// <summary>
		/// Set serialized content disposition properties
		/// </summary>
		/// <param name="serializedCd">original content disposition</param>
		public void SetContentDisposition(ContentDisposition serializedCd)
		{
			serializedCd.CreationDate = creationDate;
			serializedCd.FileName = fileName;
			serializedCd.DispositionType = dispositionType;
			serializedCd.ModificationDate = modificationDate;
			serializedCd.Inline = inline;
			serializedColParameters.SetCollection(serializedCd.Parameters);
			serializedCd.ReadDate = readDate;
			serializedCd.Size = size;
		}
	}

	/// <summary>
	/// Serializable Content Type
	/// </summary>
	[Serializable]
	public class SerializableContentType
	{
		private String boundary;

		private String charSet;

		private String mediaType;

		private String name;

		private SerializableCollection serializedColParameters;

		/// <summary>
		/// Returns serializable content type
		/// </summary>
		/// <param name="contentType">Original content type</param>
		/// <returns>serialized content type</returns>
		public static SerializableContentType GetSerializableContentType(ContentType contentType)
		{
			if (contentType == null) return null;

			var serializedContentType = new SerializableContentType
			{
				name = contentType.Name,
				boundary = contentType.Boundary,
				mediaType = contentType.MediaType,
				charSet = contentType.CharSet,
				serializedColParameters = SerializableCollection.GetSerializableCollection(contentType.Parameters)
			};

			return serializedContentType;
		}

		/// <summary>
		/// Returns Original ContentType
		/// </summary>
		/// <returns></returns>
		public ContentType GetOriginalContentType()
		{
			var serializedContentType = new ContentType { Boundary = this.boundary, CharSet = this.charSet, MediaType = this.mediaType, Name = this.name };

			serializedColParameters.SetCollection(serializedContentType.Parameters);

			return serializedContentType;
		}
	}

	/// <summary>
	/// Serializable Attachment
	/// </summary>
	[Serializable]
	public class SerializableAttachment
	{
		private String contentId;

		private SerializableContentDisposition serializedContentDisposition;

		private SerializableContentType serializedContentType;

		private Stream serializedContentStream;

		private TransferEncoding transferEncoding;

		private String name;

		private Encoding nameEncoding;

		/// <summary>
		/// Returns serializable attachment
		/// </summary>
		/// <param name="att"></param>
		/// <returns></returns>
		public static SerializableAttachment GetSerializeableAttachment(Attachment att)
		{
			if (att == null) return null;

			var serializedAttachment = new SerializableAttachment
			{
				serializedContentDisposition = SerializableContentDisposition.GetSerializableContentDisposition(att.ContentDisposition),
				contentId = att.ContentId
			};

			if (att.ContentStream != null)
			{
				var bytes = new byte[att.ContentStream.Length];
				att.ContentStream.Read(bytes, 0, bytes.Length);

				serializedAttachment.serializedContentStream = new MemoryStream(bytes);
			}

			serializedAttachment.name = att.Name;
			serializedAttachment.serializedContentType = SerializableContentType.GetSerializableContentType(att.ContentType);
			serializedAttachment.nameEncoding = att.NameEncoding;
			serializedAttachment.transferEncoding = att.TransferEncoding;
			return serializedAttachment;
		}

		/// <summary>
		/// Returns original attachment
		/// </summary>
		/// <returns></returns>
		public Attachment GetOriginalAttachment()
		{
			var attachment = new Attachment(serializedContentStream, name) { ContentId = this.contentId };
			this.serializedContentDisposition.SetContentDisposition(attachment.ContentDisposition);

			attachment.Name = name;
			attachment.ContentType = serializedContentType.GetOriginalContentType();
			attachment.NameEncoding = nameEncoding;
			attachment.TransferEncoding = transferEncoding;
			return attachment;
		}
	}


	/// <summary>
	/// Serializable Collection
	/// </summary>
	[Serializable]
	public class SerializableCollection
	{
		private readonly Dictionary<String, String> collection = new Dictionary<String, String>();

		public static SerializableCollection GetSerializableCollection(NameValueCollection col)
		{
			if (col == null) return null;

			var scol = new SerializableCollection();
			foreach (String key in col.Keys)
			{
				scol.collection.Add(key, col[key]);
			}

			return scol;
		}

		public static SerializableCollection GetSerializableCollection(StringDictionary col)
		{
			if (col == null) return null;

			var scol = new SerializableCollection();
			foreach (String key in col.Keys)
			{
				scol.collection.Add(key, col[key]);
			}

			return scol;
		}

		public void SetCollection(NameValueCollection scol)
		{

			foreach (var key in this.collection.Keys)
			{
				scol.Add(key, this.collection[key]);
			}

		}

		public void SetCollection(StringDictionary scol)
		{

			foreach (var key in this.collection.Keys)
			{
				if (scol.ContainsKey(key)) scol[key] = this.collection[key];
				else scol.Add(key, this.collection[key]);
			}
		}
	}

	/// <summary>
	/// Serializable mail message object
	/// </summary>
	[Serializable]
	public class SerializableMailMessage
	{
		public string MailID { get; set; }

		private Boolean IsBodyHtml { get; set; }

		private String Body { get; set; }

		private SerializableMailAddress SerializedFrom { get; set; }

		private readonly List<SerializableMailAddress> serializedTo = new List<SerializableMailAddress>();

		private readonly List<SerializableMailAddress> serializedCC = new List<SerializableMailAddress>();

		private readonly List<SerializableMailAddress> serializedBcc = new List<SerializableMailAddress>();

		private readonly List<SerializableMailAddress> serializedReplyTo = new List<SerializableMailAddress>();

		private SerializableMailAddress SerializedSender { get; set; }

		private String Subject { get; set; }

		private readonly List<SerializableAttachment> serializedAttachments = new List<SerializableAttachment>();

		private readonly Encoding bodyEncoding;

		private readonly Encoding subjectEncoding;

		private readonly DeliveryNotificationOptions serializedDeliveryNotificationOptions;

		private readonly SerializableCollection serializedHeaders;

		private readonly MailPriority mailPriority;

		private readonly List<SerializableAlternateView> serializedAlternateViews = new List<SerializableAlternateView>();

		/// <summary>
		/// Serializable Mail Message Contstructor
		/// </summary>
		/// <param name="mailMessage">Original Mail Message to be serialized</param>
		public SerializableMailMessage(MailMessage mailMessage)
		{
			MailID = Guid.NewGuid().ToString();
			this.IsBodyHtml = mailMessage.IsBodyHtml;
			this.Body = mailMessage.Body;
			this.Subject = mailMessage.Subject;
			this.SerializedFrom = SerializableMailAddress.GetSerializableMailAddress(mailMessage.From);

			this.serializedTo = new List<SerializableMailAddress>();
			foreach (var ma in mailMessage.To)
			{
				this.serializedTo.Add(SerializableMailAddress.GetSerializableMailAddress(ma));
			}

			this.serializedBcc = new List<SerializableMailAddress>();
			foreach (var ma in mailMessage.Bcc)
			{
				this.serializedBcc.Add(SerializableMailAddress.GetSerializableMailAddress(ma));
			}

			this.serializedCC = new List<SerializableMailAddress>();
			foreach (var ma in mailMessage.CC)
			{
				this.serializedCC.Add(SerializableMailAddress.GetSerializableMailAddress(ma));
			}

			this.serializedReplyTo = new List<SerializableMailAddress>();
			foreach (var ma in mailMessage.ReplyToList)
			{
				this.serializedReplyTo.Add(SerializableMailAddress.GetSerializableMailAddress(ma));
			}

			this.serializedAttachments = new List<SerializableAttachment>();
			foreach (var att in mailMessage.Attachments)
			{
				this.serializedAttachments.Add(SerializableAttachment.GetSerializeableAttachment(att));
			}

			this.bodyEncoding = mailMessage.BodyEncoding;

			this.serializedDeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions;
			this.serializedHeaders = SerializableCollection.GetSerializableCollection(mailMessage.Headers);
			this.mailPriority = mailMessage.Priority;
			this.SerializedSender = SerializableMailAddress.GetSerializableMailAddress(mailMessage.Sender);
			this.subjectEncoding = mailMessage.SubjectEncoding;

			foreach (var av in mailMessage.AlternateViews)
			{
				this.serializedAlternateViews.Add(SerializableAlternateView.GetSerializableAlternateView(av));
			}
		}


		/// <summary>
		/// Returns the MailMessge object from the serializeable object
		/// </summary>
		/// <returns>Original Mail Message Object</returns>
		public MailMessage GetOriginalMailObject()
		{
			var mailMsg = new MailMessage { IsBodyHtml = this.IsBodyHtml, Body = this.Body, Subject = this.Subject };

			if (this.SerializedFrom != null) mailMsg.From = this.SerializedFrom.GetOriginalMailAddress();

			foreach (var serializedMailToAddr in this.serializedTo)
			{
				mailMsg.To.Add(serializedMailToAddr.GetOriginalMailAddress());
			}

			foreach (var serializedMailCcAddr in this.serializedCC)
			{
				mailMsg.CC.Add(serializedMailCcAddr.GetOriginalMailAddress());
			}

			foreach (var serializedMailBccAddr in this.serializedBcc)
			{
				mailMsg.Bcc.Add(serializedMailBccAddr.GetOriginalMailAddress());
			}

			foreach (var serializedMailReplyToAddr in this.serializedReplyTo)
			{
				mailMsg.ReplyToList.Add(serializedMailReplyToAddr.GetOriginalMailAddress());
			}

			foreach (var serializedAttachment in this.serializedAttachments)
			{
				mailMsg.Attachments.Add(serializedAttachment.GetOriginalAttachment());
			}

			mailMsg.BodyEncoding = this.bodyEncoding;

			mailMsg.DeliveryNotificationOptions = this.serializedDeliveryNotificationOptions;
			this.serializedHeaders.SetCollection(mailMsg.Headers);
			mailMsg.Priority = this.mailPriority;

			if (this.SerializedSender != null)
			{
				mailMsg.Sender = this.SerializedSender.GetOriginalMailAddress();
			}

			mailMsg.SubjectEncoding = this.subjectEncoding;

			foreach (var av in this.serializedAlternateViews)
			{
				mailMsg.AlternateViews.Add(av.GetOriginalAlternateView());
			}

			return mailMsg;
		}
	}
}
