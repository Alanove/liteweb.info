Create View PageDataProrpertiesView
as
select pv.PageID, p.DataPropertyName, pv.DataPropertyValue
from PageDataPropertyValue pv 
inner Join PageDataProperty p
on pv.DataPropertyID = p.DataPropertyID

---

CREATE TRIGGER Page_Delete
   ON  Pages
   AFTER DELETE
AS 
BEGIN

Delete from PageDataPropertyValue where PageId in (select PageID from deleted);

END
GO
