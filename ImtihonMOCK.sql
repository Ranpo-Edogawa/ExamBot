----------------P----------G-----------------SQL-----------------------------------------
update shop_products set unit_price=1.1*unit_price
where product_manufacturer_id=(select manufacturer_id from product_manufacturers where manufacturer_name='Orbit')
and product_title_id in (select product_title_id from product_titles where product_category_id=(select category_id from product_categories where category_name='grocery')
)

----------------P----------G-----------------SQL-----------------------------------------

select person_first_name || '   ' || person_last_name as fullname,avg((price_with_discount::decimal)*product_amount) as avg_sum from customer_order_details
inner join customer_orders using(customer_order_id)
inner join customers using(customer_id)
inner join persons on customers.customer_id=persons.person_id
group by person_id
having avg((price_with_discount::decimal)*product_amount)>200000
order by avg((price_with_discount::decimal)*product_amount) desc,fullname asc


----------------P----------G-----------------SQL-----------------------------------------


select persons.person_first_name,persons.person_last_name,product_titles.product_title from customer_order_details
inner join customer_orders  using(customer_order_id) 
inner join product_titles on product_id=product_title_id
inner join customers on customer_orders.customer_order_id=customers.customer_id
inner join persons on customers.customer_id=persons.person_id
where  persons.person_birth_date between '01-01-2000' and '01-01-2005'


----------------P----------G-----------------SQL-----------------------------------------


DO $$ 
DECLARE
    v_manufacturer_id INT;
    v_supplier_id INT;
    v_category_id INT;
    v_product_title_id INT;
BEGIN
    SELECT manufacturer_id INTO v_manufacturer_id
    FROM product_manufacturers
    WHERE manufacturer_name = 'Coca-cola company';

    SELECT supplier_id INTO v_supplier_id
    FROM product_suppliers
    WHERE supplier_name = 'Pepsi Supplier'; 

    SELECT category_id INTO v_category_id
    FROM product_categories
    WHERE category_name = 'Drinks';

    INSERT INTO product_titles (product_title, product_category_id)
    VALUES ('Pepsi', v_category_id)
    RETURNING product_title_id INTO v_product_title_id;

    INSERT INTO shop_products (product_title_id, product_manufacturer_id, product_supplier_id, unit_price, comment)
    VALUES (v_product_title_id, v_manufacturer_id, v_supplier_id, '$1.99', 'New product: Pepsi');

    COMMIT;
END $$;



----------------P----------G-----------------SQL-----------------------------------------




create view Customer_details as
select person_first_name|| ' ' || person_last_name as Full_name,
person_birth_date, cu.card_number
from persons inner join customers as cu on person_id=customer_id



----------------P----------G-----------------SQL-----------------------------------------


SELECT
  product_title_id,
  comment,
  CASE
    WHEN unit_price::decimal < 300 THEN 'very cheap'
    WHEN unit_price::decimal > 300 AND unit_price::decimal <= 750 THEN 'affordable'
    ELSE 'expensive'
  END AS type
FROM  shop_products;



----------------P----------G-----------------SQL-----------------------------------------


insert into product_categories(category_id,category_name) values(19,'unusual')

insert into product_titles(product_title_id,product_title,product_category_id) values(365,'zor narsa bu',19)

insert into product_suppliers(supplier_id,supplier_name) values(27,'Elyor')

insert into product_manufacturers(manufacturer_id,manufacturer_name) values(39,'Sirdaryolik')

insert into shop_products(product_id,product_title_id,product_manufacturer_id,product_supplier_id,unit_price,comment) 
values(99001,365,39,27,'$200000','menimcha qoshildi')




----------------P----------G-----------------SQL-----------------------------------------



CREATE OR REPLACE FUNCTION getProductListByOperationDate(operation_date date)
RETURNS TABLE (product_name varchar) AS $$
BEGIN
    RETURN QUERY 
        SELECT product_name
        FROM your_table_name
        WHERE operation_date = getProductListByOperationDate.operation_date;
END;
$$ LANGUAGE plpgsql;


----------------P----------G-----------------SQL---------FUNC----------------------------


CREATE or replace FUNCTION GETPRODUCTLISTBYOPERATIONDATE11(OPERATIONDATE date) RETURNS TABLE (P VARCHAR(255)) LANGUAGE PlpgSql AS $$
begin
return query select product_titles.product_title from customer_order_details
inner join customer_orders using(customer_order_id)
inner join product_titles on product_titles.product_title_id= customer_order_details.product_id
where DATE(operation_time)=operationDate;
end;$$;

select * from GETPRODUCTLISTBYOPERATIONDATE11('2011-03-24');



----------------P----------G-----------------SQL---------FUNC----------------------------



create view product_details  as
select pt.product_title, pc.category_name, sup.supplier_name, pm.manufacturer_name  
from shop_products as sp inner join product_titles as pt
on sp.product_title_id=pt.product_title_id inner join product_categories as pc
on pt.product_category_id = pc.category_id inner join product_suppliers as sup on 
sp.product_supplier_id=sup.supplier_id inner join product_manufacturers as pm on
sp.product_manufacturer_id = pm.manufacturer_id



















